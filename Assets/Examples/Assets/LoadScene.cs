using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DotweenScenes
{
    TweenerControl,
    Animation,
    Basics,
    Main,
    Materials,
    Sequences,
    UGUI,
    NiL
}

public class LoadScene : MonoBehaviour
{
    public DotweenScenes dotweenScenes;

    public void Load()
    {
        string sceneName = Enum.GetName(typeof(DotweenScenes), dotweenScenes);
        SceneManager.LoadScene(sceneName);
    }
}
