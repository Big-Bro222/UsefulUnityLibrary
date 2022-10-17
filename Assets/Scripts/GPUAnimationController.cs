using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GPUAnimationController : MonoBehaviour
{
    public GPUAnimationClip _defaultAnimationClip;
    [HideInInspector] public bool isGenerationCompleted=false;
    public List<GPUAnimationClip> animationClips=new List<GPUAnimationClip>();
    Material _material;
    public bool Random = false;
    
    [SerializeField,Range(0,1)]
    float animationOffset = 0;

    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        if (Random)
        {
            float randomTime= UnityEngine.Random.Range(0f,1f);
            animationOffset= randomTime;
        }

        assignAnimationClip(_defaultAnimationClip, animationOffset);
        
    }

    public void Play(string animationName, float withOffset = 0)
    {
        for(int i=0;i< animationClips.Count; i++)
        {
            if (animationName.Equals(animationClips[i].name))
            {
                assignAnimationClip(animationClips[i],withOffset);
                return;
            }
        }
        Debug.LogError("cannot Find animationClip");
    }

    public void SetAnimationOffset(float withOffset)
    {
        animationOffset = withOffset;
        _material.SetFloat("_Offset", withOffset);
    }

    private void assignAnimationClip(GPUAnimationClip gPUAnimationClip,float withOffset)
    {
        _material.SetTexture("_MainTex", gPUAnimationClip.skinTexture);
        _material.SetTexture("_AnimTex", gPUAnimationClip.animTexture);
        _material.SetInt("_BoneCount", gPUAnimationClip.boneCount);
        _material.SetInt("_FrameCount", gPUAnimationClip.FrameCount);
        _material.SetInt("_FrameRate", gPUAnimationClip.FrameRate);
        _material.SetInt("_Start", gPUAnimationClip.StartFrame);
        if (withOffset != 0)
        {
            _material.SetFloat("_Offset", withOffset);
        }
    }
}

