using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleController : MonoBehaviour
{
    GPUAnimationController gpuController;
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        gpuController = GetComponent<GPUAnimationController>();
    }

    public void Play(string clipName)
    {
        gpuController.Play(clipName);
    }
}
