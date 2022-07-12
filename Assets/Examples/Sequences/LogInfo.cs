using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogInfo : MonoBehaviour
{
    public Transform ScrollParent;
    public GameObject textinfo;

    public void Log(string content)
    {
        GameObject logitem=Instantiate(textinfo, ScrollParent);
        logitem.GetComponent<TextMeshProUGUI>().text = content+ " "+ScrollParent.childCount;
    }
}
