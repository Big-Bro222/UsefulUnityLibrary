using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GPU Skinning/Create GPUAnimationClip")]
public class GPUAnimationClip : ScriptableObject
{
    public Texture2D animTexture;
    public int boneCount = 0;
    public int FrameCount = 0;
    public int FrameRate = 30;
    public int StartFrame = 0;
}

