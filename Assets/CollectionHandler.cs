using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public enum Collection
{
    Dictionary,
    SortedDictionary,
    SortedList,
    List
}
public class CollectionHandler : MonoBehaviour
{
    public Collection collectionType;
    void Start()
    {
        switch (collectionType)
        {
            case Collection.Dictionary:
                PrintDictionary();
                break;
            case Collection.SortedDictionary:
                PrintSortedDictionary();
                break;
            case Collection.SortedList:
                PrintSortedList();
                break;
            case Collection.List:
                PrintList();
                break;
            default:
                print("Please assign a collectionType first");
                break;
        }
    }

    void PrintSortedDictionary()
    {
        SortedDictionary<int, string> sd = new SortedDictionary<int, string>();
        sd.Add(9, "��");
        sd.Add(8, "��");
        sd.Add(7, "��");
        sd.Add(6, "��");
        sd.Add(1, "һ");
        sd.Add(5, "��");
        sd.Add(3, "��");
        sd.Add(2, "��");
        Debug.Log("-----SortedDictionary-----");
        //print the dictionary
        foreach (var item in sd)
        {
            string info = string.Format(" Key = {0}, Value = {1}", item.Key, item.Value);
            Debug.Log(info);
        }
    }

    void PrintSortedList()
    {
        SortedList<int, string> sd = new SortedList<int, string>();
        sd.Add(9, "��");
        sd.Add(8, "��");
        sd.Add(7, "��");
        sd.Add(6, "��");
        sd.Add(1, "һ");
        sd.Add(5, "��");
        sd.Add(3, "��");
        sd.Add(2, "��");
        Debug.Log("-----SortedList-----");
        //print the dictionary
        foreach (var item in sd)
        {
            string info = string.Format(" Key = {0}, Value = {1}", item.Key, item.Value);
            Debug.Log(info);
        }

        Debug.Log("Key 7 is has the index of"+sd.IndexOfKey(7));
        //remove the item with biggest key
        sd.RemoveAt(sd.Count - 1);
        Debug.Log("-----SortedList After remove the last pair-----");
        foreach (var item in sd)
        {
            string info = string.Format(" Key = {0}, Value = {1}", item.Key, item.Value);
            Debug.Log(info);
        }

        string value=(sd.Values)[3];
        Debug.Log("the forth value is " + value);       
    }

    void PrintDictionary()
    {
        Dictionary<int, string> d = new Dictionary<int, string>();
        d.Add(9, "��");
        d.Add(8, "��");
        d.Add(7, "��");
        d.Add(6, "��");
        d.Add(1, "һ");
        d.Add(5, "��");
        d.Add(3, "��");
        d.Add(2, "��");
        //print the dictionary
        Debug.Log("-----Dictionary-----");
        foreach (var item in d)
        {
            string info = string.Format(" Key = {0}, Value = {1}", item.Key, item.Value);
            Debug.Log(info);
        }
    }

    void PrintList()
    {
        List <string> sd = new List <string>();
        sd.Add("��");
        Debug.Log(string.Format("List contains {0} items, the Capacity of the List is {1}",sd.Count,sd.Capacity));
        sd.Add("��");
        Debug.Log(string.Format("List contains {0} items, the Capacity of the List is {1}", sd.Count, sd.Capacity));

        sd.Add( "��"); 
        Debug.Log(string.Format("List contains {0} items, the Capacity of the List is {1}", sd.Count, sd.Capacity));

        sd.Add( "��");
        Debug.Log(string.Format("List contains {0} items, the Capacity of the List is {1}", sd.Count, sd.Capacity));

        sd.Add("һ");
        Debug.Log(string.Format("List contains {0} items, the Capacity of the List is {1}", sd.Count, sd.Capacity));

        sd.Add("��");
        Debug.Log(string.Format("List contains {0} items, the Capacity of the List is {1}", sd.Count, sd.Capacity)); 
        
        sd.Add("��");
        Debug.Log(string.Format("List contains {0} items, the Capacity of the List is {1}", sd.Count, sd.Capacity)); 
        
        sd.Add("��");
        Debug.Log(string.Format("List contains {0} items, the Capacity of the List is {1}", sd.Count, sd.Capacity));

        sd.Add("ʮ");
        Debug.Log(string.Format("List contains {0} items, the Capacity of the List is {1}", sd.Count, sd.Capacity));

        sd.TrimExcess();
        Debug.Log(string.Format("After TrimExcess, List contains {0} items, the Capacity of the List is {1}", sd.Count, sd.Capacity));

        Debug.Log("-----List-----");
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var item in sd)
        {
            stringBuilder.Append(item);
            stringBuilder.Append(" ");
        }
        Debug.Log(stringBuilder.ToString());

    }
}
