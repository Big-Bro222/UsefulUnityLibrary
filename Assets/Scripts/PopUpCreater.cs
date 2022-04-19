using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpCreater : MonoSingleton<PopUpCreater>
{
    GameObject messagePrefab;
    private void Start()
    {
        messagePrefab = Resources.Load<GameObject>("PopUp");
    }
    public void PopUp(string content,string caption,InfoStatus infoStatus)
    {
        GameObject popUpGo=Instantiate(messagePrefab, transform);
        popUpGo.GetComponent<PopUp>().SetText(content,caption,infoStatus);
    }
}
