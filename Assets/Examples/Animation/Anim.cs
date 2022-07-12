using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class Anim : MonoBehaviour
{
    [SerializeField] Transform AnimParent;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < AnimParent.childCount; i++)
        {
            Ease EaseType = (Ease)i;
            AnimParent.GetChild(i).GetComponent<EaseMode>().SetEase(EaseType);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GotoUrl()
    {
        Application.OpenURL("https://easings.net/");
    }
}
