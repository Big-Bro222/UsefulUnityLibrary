using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputGrid : MonoBehaviour
{
    public Button AddButton;
    public Button DeleteButton;
    public string previousText;
    public void Clear()
    {
        GetComponent<InputField>().text = "";
        AddButton.gameObject.SetActive(false);
        DeleteButton.gameObject.SetActive(false);
    }
}
