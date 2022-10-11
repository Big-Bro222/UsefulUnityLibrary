using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUAnimationController : MonoBehaviour
{
    [SerializeField] GPUAnimationClip _animationClip;
    Material _material;
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        _material.SetTexture("_AnimTex", _animationClip.animTexture);
        _material.SetInt("_BoneCount", _animationClip.boneCount);
        _material.SetInt("_FrameCount", _animationClip.FrameCount);
        _material.SetInt("_FrameRate", _animationClip.FrameRate);
        _material.SetInt("_Start",_animationClip.StartFrame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

