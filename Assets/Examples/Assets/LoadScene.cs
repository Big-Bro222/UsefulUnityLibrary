using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DotweenScenes
{
    TweenerControl,
    Animation,
    MoveMent,
    Main,
    Materials,
    Sequences,
    UGUI,
    Paths,
    NiL
}

public class LoadScene : MonoBehaviour
{
    public DotweenScenes dotweenScenes;

    public void Load()
    {
        string sceneName = Enum.GetName(typeof(DotweenScenes), dotweenScenes);
        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
