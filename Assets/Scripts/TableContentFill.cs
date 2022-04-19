using DataBase;
using System;
using System.Collections;
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
            inputField.text = "";
        }
    }

    public void RefreshTable()
    {
        ClearTable();
        ShowTable<ExampleClassC>("UserProfile");
    }
    private void FillTableContent<T>(List<T> ClassList) where T : class
    {
        dataList = ClassList;
        int firstRowindex = 0;
        int firstColindex = 0;
        Fields = typeof(T).GetFields();
        //SetUp title
        for (int i = 0; i < Fields.Length; i++)
        {
            string name = Fields[i].GetCustomAttribute<ModelHelp>().FieldName;
            SetTableTxt(firstRowindex, i + 1, name, Fields[i].GetCustomAttribute<ModelHelp>().IsPrimaryKey);
            GetGridInput(firstRowindex, i + 1).interactable = false;
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
        for (int j = 0; j < ClassList.Count; j++)
        {
            SetTableTxt(j + 1, 0, j.ToString());
            GetGridInput(j + 1, 0).interactable = false;
            for (int i = 0; i < Fields.Length; i++)
            {
                string Value = Fields[i].GetValue(ClassList[j]).ToString();
                SetTableTxt(j + 1, i + 1, Value,Fields[i].GetCustomAttribute<ModelHelp>().IsPrimaryKey);
            }
        }
    }

    private InputField GetGridInput(int row, int col)
    {
        return tMP_InputFields[row * colNum + col];
    }

    private void SetTableTxt(int row, int col, string txt)
    {
        InputField input = GetGridInput(row, col);
        input.text = txt;
        string currentText = txt;
    }

    private void SetTableTxt(int row, int col, string txt, bool isPrimary)
    {
        InputField input = GetGridInput(row, col);
        input.text = txt;
        string currentText = txt;
        if (isPrimary)
        {
            input.textComponent.color = Color.red;
        }
        else
        {
            input.textComponent.color = Color.black;
        }

        input.onEndEdit.AddListener(
            (string value) =>
            {
                Debug.Log(isPrimary);
                //if value didn't change,return
                if (value.Equals(currentText))
                {
                    return;
                }

                //if PrimaryValue is empty, log Error
                if (isPrimary && value.Equals(""))
                {
                    PopUpCreater.Instance.PopUp("Primary value cannot be null!", "Insert Fail", InfoStatus.Error);
                    input.text = currentText;
                    return;
                }

                FieldInfo fieldInfo = Fields[col - 1];
                fieldInfo.SetValue(dataList.ElementAtOrDefault(row - 1), value);
                sql.Insert<ExampleClassC>((ExampleClassC)dataList.ElementAtOrDefault(row - 1), tableName);
                //dataList(row)
                Debug.Log("Change the column of " + fieldInfo.Name + " to " + value);
                PopUpCreater.Instance.PopUp("Change the column of " + fieldInfo.Name + " from " + currentText + " to " + value, "Insert Success", InfoStatus.Success);
            }
            );
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
