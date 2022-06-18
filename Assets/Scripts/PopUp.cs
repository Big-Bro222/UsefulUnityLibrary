using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public enum InfoStatus
{
    Success,
    Warning,
    Error
}

public class PopUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] TextMeshProUGUI captionTextMesh;
    [SerializeField] TextMeshProUGUI timeTextMesh;

    // Start is called before the first frame update
    public void SetText(string txt,string caption,InfoStatus info)
    {
        textMesh.text = txt;
        captionTextMesh.text = caption;        
        timeTextMesh.text = DateTime.Now.ToString();
        switch (info)
        {
            case InfoStatus.Success:
                captionTextMesh.color = Color.green;
                break;
            case InfoStatus.Warning:
                captionTextMesh.color = Color.yellow;
                break;
            case InfoStatus.Error:
                captionTextMesh.color = Color.red;
                break;
            default:
                captionTextMesh.color = Color.gray;
                break;
        }
    }

    //void Start()
    //{
    //    Invoke("DestroyPopUp", 5f);
    //}

    public void DestroyPopUp()
    {
        Destroy(gameObject);
    }
}
