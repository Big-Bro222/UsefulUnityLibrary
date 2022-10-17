using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUAnimationController : MonoBehaviour
{
    public GPUAnimationClip _defaultAnimationClip;
    public List<GPUAnimationClip> animationClips=new List<GPUAnimationClip>();
    Material _material;
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        _material.SetTexture("_AnimTex", _defaultAnimationClip.animTexture);
        _material.SetInt("_BoneCount", _defaultAnimationClip.boneCount);
        _material.SetInt("_FrameCount", _defaultAnimationClip.FrameCount);
        _material.SetInt("_FrameRate", _defaultAnimationClip.FrameRate);
        _material.SetInt("_Start",_defaultAnimationClip.StartFrame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

