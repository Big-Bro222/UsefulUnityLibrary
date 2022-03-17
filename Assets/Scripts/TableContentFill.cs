using DataBase;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class TableContentFill : MonoBehaviour
{
    SqlDbCommand sql;
    Transform table;
    Transform tabsContainer;
    //the const column number
    int colNum = 10;
    private void Start()
    {
        table = transform.Find("TableContent");
        tabsContainer = transform.Find("TabsContainer");

        

        //Create SqliteConnection
        string dbPath = Application.persistentDataPath + "/Save/SaveData.db";
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
        ShowTable<ExampleClassB>("UserOutfit");
    }

    private void ShowTable<T>(string TableName) where T : class
    {
        sql.CreateTable<T>(TableName);
        List<T> ClassCList = sql.SelectAll<T>(TableName);
        FillTableContent(ClassCList);
    }

    private void FillTableContent<T>(List<T> ClassList) where T : class
    {
        int row = 0;
        int col = 0;
        FieldInfo[] Fields = typeof(T).GetFields();
        //SetUp title
        for (int i = 0; i < Fields.Length; i++)
        {
            string name = Fields[i].GetCustomAttribute<ModelHelp>().FieldName;
            SetTableTxt(row, i + 1, name);
        }

        for (int j = 0; j < ClassList.Count; j++)
        {
            SetTableTxt(j + 1, 0, j.ToString());
            for (int i = 0; i < Fields.Length; i++)
            {
                string Value = Fields[i].GetValue(ClassList[j]).ToString();
                SetTableTxt(j + 1, i + 1, Value);
            }
        }
    }

    private void SetTableTxt(int row, int col, string txt)
    {
        TMP_InputField input = table.GetChild(row * 10 + col).GetComponent<TMP_InputField>();
        input.text = txt;
        if (row == 0 || col == 0)
        {
            input.interactable = false;
        }
    }
}
