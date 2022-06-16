using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableContentFill : MonoBehaviour
{
    SqlDbCommand sql;
    Transform table;
    Transform tabsContainer;
    [SerializeField] TMP_InputField sqlInput;
    [SerializeField] Button submitBtn;
    InputField[] tMP_InputFields;
    FieldInfo[] Fields;
    FieldInfo primaryInfo;
    string tableName = "UserProfile";
    //the const column number
    int colNum = 10;
    int rowNum = 10;

    int classCount = 0;
    int classFieldCount = 0;
    IEnumerable<System.Object> dataList;
    private void Start()
    {
        table = transform.Find("TableContent");
        tabsContainer = transform.Find("TabsContainer");
        tMP_InputFields = table.GetComponentsInChildren<InputField>();
        submitBtn.onClick.AddListener(SelectColumnBySql);


        //Create SqliteConnection
        string dbPath = Application.dataPath + "/Save/SaveData.db";
        sql = new SqlDbCommand(dbPath);
        List<string> Tables = sql.QueryAllTable();
        GameObject tabPrefab = Resources.Load<GameObject>("TableTabBtn");
        foreach (string tableName in Tables)
        {
            GameObject tabBtn = Instantiate(tabPrefab, tabsContainer);
            tabBtn.GetComponentInChildren<TextMeshProUGUI>().text = tableName;
            //bind btn action
        }

        //Show the table
        ShowTable<ExampleClassC>(tableName);
    }

    private void ShowTable<T>(string TableName) where T : class
    {
        //Check if the table is created
        if (!sql.IsTableCreated<T>(TableName))
        {
            Debug.LogError("Cannot find table " + TableName + " ,please create it first.");
            return;
        }
        List<T> ClassList = sql.SelectAll<T>(TableName);
        classCount = ClassList.Count;
        FillTableContent(ClassList);
    }

    private void ClearTable()
    {
        foreach (InputField inputField in tMP_InputFields)
        {
            inputField.GetComponent<InputGrid>().Clear();
        }
    }

    public void RefreshTable()
    {
        ClearTable();
        ShowTable<ExampleClassC>("UserProfile");
    }
    private void FillTableContent<T>(List<T> ClassList) where T : class
    {
        //disable all the inputfield at first and then enable them.
        foreach (InputField inputField in tMP_InputFields)
        {
            inputField.interactable = false;
        }

        dataList = ClassList;
        int firstRowindex = 0;
        int firstColindex = 0;
        int primaryKeyCol = -1;
        Fields = typeof(T).GetFields();
        //SetUp title
        for (int i = 0; i < Fields.Length; i++)
        {
            string name = Fields[i].GetCustomAttribute<ModelHelp>().FieldName;
            if (Fields[i].GetCustomAttribute<ModelHelp>().IsPrimaryKey)
            {
                primaryKeyCol = i + 1;
                primaryInfo = Fields[i];
            }
            SetTableTxt(firstRowindex, i + 1, name, Fields[i].GetCustomAttribute<ModelHelp>().IsPrimaryKey);
        }

        //disable first row and first col
        for (int i = 0; i < colNum - 1; i++)
        {
            GetGridInput(firstRowindex, i + 1).interactable = false;
        }
        for (int i = 0; i < rowNum - 1; i++)
        {
            GetGridInput(i + 1, firstColindex).interactable = false;
        }

        //fill class numbers
        for (int row = 0; row < ClassList.Count; row++)
        {
            SetTableTxt(row + 1, 0, row.ToString());
            GetGridInput(row + 1, 0).interactable = false;
            for (int col = 0; col < Fields.Length + 1; col++)
            {
                if (col < Fields.Length)
                {
                    string Value = Fields[col].GetValue(ClassList[row]).ToString();
                    SetTableTxt(row + 1, col + 1, Value, Fields[col].GetCustomAttribute<ModelHelp>().IsPrimaryKey);
                }
                else
                {
                    Button deleteBtn = GetGridInput(row + 1, col + 1).GetComponent<InputGrid>().DeleteButton;
                    deleteBtn.gameObject.SetActive(true);
                    int temp = row;
                    deleteBtn.onClick.AddListener(() =>
                    {
                        DeleteData(temp);
                        deleteBtn.onClick.RemoveAllListeners();
                        RefreshTable();
                    });
                }
            }

        }

        //Add a button in the remain column for add new Input
        InputField addInput = GetGridInput(ClassList.Count + 1, 0);
        Button addButton = addInput.GetComponent<InputGrid>().AddButton;
        addButton.gameObject.SetActive(true);
        addButton.onClick.AddListener(() => OnClickAdd(addInput, ClassList.Count, primaryKeyCol, addButton));
    }

    private void DeleteData(int row)
    {
        string colName = primaryInfo.Name;
        string colValue = primaryInfo.GetValue(dataList.ElementAtOrDefault(row)).ToString();
        sql.DeleteByCol<ExampleClassC>(colName, colValue, tableName);
        
    }

    private void OnClickAdd(InputField addInput, int ClassListCount, int primaryKeyCol, Button addButton)
    {
        addInput.text = ClassListCount.ToString();
        InputField input = GetGridInput(ClassListCount + 1, primaryKeyCol);
        input.interactable = true;
        input.textComponent.color = Color.red;
        input.Select();
        //input.OnSelect();
        input.onEndEdit.AddListener((string value) =>
        {
            //detect null and resume the states
            //if PrimaryValue is empty, log Error
            if (value.Equals(""))
            {
                PopUpCreater.Instance.PopUp("Primary value cannot be null!", "Insert Fail", InfoStatus.Error);
                //set things back
                addInput.text = "";
                addButton.gameObject.SetActive(true);
                addButton.onClick.AddListener(() => OnClickAdd(addInput, ClassListCount, primaryKeyCol, addButton));
                input.interactable = false;
                input.onEndEdit.RemoveAllListeners();
                return;
            }
            //detect new data insert
            ExampleClassC data = Activator.CreateInstance<ExampleClassC>();
            dataList= dataList.Concat(new[] { data });
            primaryInfo.SetValue(data, value);
            sql.Insert<ExampleClassC>(data, tableName);
            for (int i = 0; i < Fields.Length; i++)
            {
                //set the input on the new line to interactable
                int row = ClassListCount + 1;
                int col = i + 1;
                bool isPrimary = col.Equals(primaryKeyCol);
                InputField inputsInline = GetGridInput(ClassListCount + 1, i + 1);
                inputsInline.interactable = true;
                inputsInline.onEndEdit.AddListener((string value) => UpdateDataBase(value, "", col, row, isPrimary, inputsInline));
            }
            //Add add icon, Add deleteBtn
            Button deleteBtn = GetGridInput(ClassListCount + 1, Fields.Length + 1).GetComponent<InputGrid>().DeleteButton;
            deleteBtn.gameObject.SetActive(true);
            int temp = ClassListCount;
            deleteBtn.onClick.AddListener(() =>
            {
                DeleteData(temp);
                deleteBtn.onClick.RemoveAllListeners();
                RefreshTable();
            });

            InputField nextaddInput = GetGridInput(dataList.Count() + 1, 0);
            Button nextaddButton = nextaddInput.GetComponent<InputGrid>().AddButton;
            nextaddButton.gameObject.SetActive(true);
            nextaddButton.onClick.AddListener(() => OnClickAdd(nextaddInput, dataList.Count(), primaryKeyCol, nextaddButton));

            PopUpCreater.Instance.PopUp("Add new line with primary key " + value + " please fillin rest of the data", "Add data Success", InfoStatus.Success);
            input.onEndEdit.RemoveAllListeners();
        });
        addButton.onClick.RemoveAllListeners();
        addButton.gameObject.SetActive(false);
    }

    private InputField GetGridInput(int row, int col)
    {
        return tMP_InputFields[row * colNum + col];
    }

    private void SetTableTxt(int row, int col, string txt)
    {
        InputField input = GetGridInput(row, col);
        input.text = txt;
    }

    private void SetTableTxt(int row, int col, string txt, bool isPrimary)
    {
        InputField input = GetGridInput(row, col);
        input.interactable = true;
        input.text = txt;
        //get the text content for later usage
        string currentText = txt;
        if (isPrimary)
        {
            input.textComponent.color = Color.red;
        }
        else
        {
            input.textComponent.color = Color.black;
        }
        //when the input changes, update the DataBase
        input.onEndEdit.AddListener(
            (string value) => UpdateDataBase(value, currentText, col, row, isPrimary, input)
            );
    }

    private void UpdateDataBase(string value, string previousText, int col, int row, bool isPrimary, InputField input)
    {
        //if value didn't change,return
        if (value.Equals(previousText))
        {
            return;
        }
        //if PrimaryValue is empty, log Error
        if (isPrimary && value.Equals(""))
        {
            PopUpCreater.Instance.PopUp("Primary value cannot be null!", "Insert Fail", InfoStatus.Error);
            input.text = previousText;
            return;
        }
        FieldInfo fieldInfo = Fields[col - 1];
        fieldInfo.SetValue(dataList.ElementAtOrDefault(row - 1), value);
        sql.Insert<ExampleClassC>((ExampleClassC)dataList.ElementAtOrDefault(row - 1), tableName);
        PopUpCreater.Instance.PopUp("Change the column of " + fieldInfo.Name + " from " + previousText + " to " + value, "Insert Success", InfoStatus.Success);
    }

    public void SelectColumnBySql()
    {
        string sqlCommand = sqlInput.text;
        string[] sqlArray = sqlCommand.Split(',');
        List<string> columnList = sql.SelectColumnBySql<string>(sqlArray[0], sqlArray[1], tableName);
        RefreshTable();
        FillTableContent(columnList);
    }


}
