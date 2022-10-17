using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

internal struct VertexInfo
{
    public Vector3 position;
    public Vector3 normal;
}

public class GPUBaker : MonoBehaviour
{
    public enum BAKEMODE
    {
        bone,
        vertex
    }

    public enum UVCHANNEL
    {
        UV,
        UV2,
        UV3
    }

    public UVCHANNEL outW;
    public UVCHANNEL outI;
    public Mesh mesh;

    [HideInInspector] public ComputeShader computeShader;
    [HideInInspector] public Button button;

    private void OnValidate()
    {

    }
    public void Bake()
    {
        if (mesh == null) throw new Exception("请添加Mesh");

        if (outI == outW) throw new Exception("频道冲突，请重选");

        BakeBoneTexture();
    }

    private void BakeBoneTexture()
    {

        var animator = GetComponent<Animator>();
        var clips = animator.runtimeAnimatorController.animationClips;
        var skin = GetComponentInChildren<SkinnedMeshRenderer>();
        var boneCount = mesh.bindposes.Length;

        animator.speed = 1;
        var textWidth = boneCount;
        List<AnimationInfo> animationInfos = new List<AnimationInfo>();
        List<Texture2D> textures = new List<Texture2D>();
        List<int> frameCounts = new List<int>();
        List<int> frameRates = new List<int>();

        ///Mapping UV to a new Mesh
        var bakedMesh = new Mesh();
        bakedMesh = Instantiate(mesh);
        bakedMesh.name = string.Format("{0}.mesh", name);
        MappingBoneIndexAndWeightToMeshUV(bakedMesh, UVChannel.UV2, UVChannel.UV3);
        
        List<GPUAnimationClip> gpuClips = new List<GPUAnimationClip>();

        foreach (var clip in clips)
        {
            Debug.Log(clip.name);
            int frameCount = (int)(clip.frameRate * clip.length);
            Texture2D boneTex = CreateBoneTex(animator, skin, clip, mesh, 512, frameCount);
            boneTex.name = string.Format("{0}", clip.name);
            textures.Add(boneTex);
            frameCounts.Add(frameCount);
            frameRates.Add((int)clip.frameRate);
            /*Useless, used for demo
              SaveAsJPG(boneTex, Path.Combine("Assets/BakedDemoImgs"), boneTex.name);
            */
            //Get singe baked matrixs
            AssetDatabase.CreateAsset(boneTex, Path.Combine("Assets/BakedMatrixs", boneTex.name + ".asset"));

            GPUAnimationClip gpuClip = ScriptableObject.CreateInstance<GPUAnimationClip>();
            gpuClip.boneCount = boneCount;
            gpuClip.StartFrame = 0;
            gpuClip.FrameCount = frameCount;
            gpuClip.FrameRate = (int) clip.frameRate;
            gpuClip.animTexture = boneTex;
            gpuClips.Add(gpuClip);
            AssetDatabase.CreateAsset(gpuClip, Path.Combine("Assets/AnimationClips", clip.name + ".asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        GameObject prefab = new GameObject(name + "_Baked");
        MeshFilter meshFilter =prefab.AddComponent<MeshFilter>();
        meshFilter.mesh = bakedMesh;
        MeshRenderer meshRenderer = prefab.AddComponent<MeshRenderer>();
        Material mat = new Material(Shader.Find("Custom/BoneAnimationShader"));
        meshRenderer.material = mat;
        GPUAnimationController ac = prefab.AddComponent<GPUAnimationController>();
        ac.animationClips = gpuClips;
        ac._defaultAnimationClip = gpuClips[0];

        var start = 0;
        var height = 0;
        for (int i = 0; i < textures.Count; ++i)
        {
            start = height;
            height += textures[i].height;
            AnimationInfo info = new AnimationInfo(textures[i].name, start, height - 1, frameCounts[i], frameRates[i]);
            animationInfos.Add(info);
        }

        AssetDatabase.CreateAsset(bakedMesh, Path.Combine("Assets/BakedMesh", name + ".BakedMesh" + ".mesh"));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private Texture2D combineTextures(List<Texture2D> textures)
    {
        var height = 0;
        var start = 0;
        foreach (var texture in textures)
        {
            start = height;
            height += texture.height;
        }

        var result = new Texture2D(512, height, TextureFormat.RGBA32, false);
        height = 0;
        foreach (var texture in textures)
        {
            result.SetPixels(0, height, texture.width, texture.height, texture.GetPixels());
            height += texture.height;
        }
        return result;
    }
    public void SaveAsJPG(Texture2D texture2D, string contents, string pngName)
    {
        var bytes = texture2D.EncodeToJPG();
        var file = File.Open(contents + "/" + pngName + ".jpg", FileMode.Create);
        var writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        writer.Close();
    }

    private Texture2D CreateBoneTex(Animator animator, SkinnedMeshRenderer skinnedMeshRenderer, AnimationClip clip,
        Mesh mesh,
        int width, int animFrameCount)
    {
        var bindPoses = mesh.bindposes;
        Debug.Log(bindPoses.Length);
        var bones = skinnedMeshRenderer.bones;

        var bonesCount = bones.Length;
        if (bindPoses.Length != bones.Length)
            return null;
        animator.applyRootMotion = false;
        animator.Play(clip.name);

        // Start sampling

        //4x4matrix of 3D translation,only need to save the first three rows, the last one is always (0,0,0,1)
        int lines = Mathf.CeilToInt((float)bones.Length * animFrameCount * 12 / width);
        var result = new Texture2D(width, lines, TextureFormat.RGBA32, false);
        result.filterMode = FilterMode.Point;
        result.wrapMode = TextureWrapMode.Clamp;
        //4x4matrix of 3D translation,only need to save the first three rows, the last one is always (0,0,0,1)
        var colors = new Color[width * lines * 3];
        // get data from each frame
        for (var i = 0; i <= animFrameCount; i++)
        {
            //locate to the frame
            clip.SampleAnimation(gameObject, i / clip.frameRate);


            // Convert the twelve float to RGBA to save precision accuracy.
            for (var j = 0; j < bonesCount; j++)
            {
                var matrix = transform.worldToLocalMatrix * bones[j].localToWorldMatrix * bindPoses[j];

                colors[(i * bonesCount + j) * 12 + 0] = EncodeFloatRGBA(matrix.m00);
                colors[(i * bonesCount + j) * 12 + 1] = EncodeFloatRGBA(matrix.m01);
                colors[(i * bonesCount + j) * 12 + 2] = EncodeFloatRGBA(matrix.m02);
                colors[(i * bonesCount + j) * 12 + 3] = EncodeFloatRGBA(matrix.m03);

                colors[(i * bonesCount + j) * 12 + 4] = EncodeFloatRGBA(matrix.m10);
                colors[(i * bonesCount + j) * 12 + 5] = EncodeFloatRGBA(matrix.m11);
                colors[(i * bonesCount + j) * 12 + 6] = EncodeFloatRGBA(matrix.m12);
                colors[(i * bonesCount + j) * 12 + 7] = EncodeFloatRGBA(matrix.m13);

                colors[(i * bonesCount + j) * 12 + 8] = EncodeFloatRGBA(matrix.m20);
                colors[(i * bonesCount + j) * 12 + 9] = EncodeFloatRGBA(matrix.m21);
                colors[(i * bonesCount + j) * 12 + 10] = EncodeFloatRGBA(matrix.m22);
                colors[(i * bonesCount + j) * 12 + 11] = EncodeFloatRGBA(matrix.m23);
            }
        }

        //left to right, bottom to top
        result.SetPixels(colors);
        result.Apply();

        return result;
    }

    private static Vector4 EncodeFloatRGBA(float v)
    {
        v = v * 0.01f + 0.5f;
        var kEncodeMul = new Vector4(1.0f, 255.0f, 65025.0f, 160581375.0f);
        var kEncodeBit = 1.0f / 255.0f;
        var enc = kEncodeMul * v;
        for (var i = 0; i < 4; i++)
            enc[i] = enc[i] - Mathf.Floor(enc[i]);
        enc = enc - new Vector4(enc.y, enc.z, enc.w, enc.w) * kEncodeBit;
        return enc;
    }

    public static bool MappingBoneIndexAndWeightToMeshUV(Mesh mesh, UVChannel indexChannel, UVChannel weightChannel)
    {

        var boneWeights = mesh.boneWeights;
        var weightUV = new List<Vector2>();
        var indexUV = new List<Vector2>();

        Debug.Log(mesh.vertices.Length);
        Debug.Log(boneWeights.Length + " weights");
        for (var i = 0; i < boneWeights.Length; i++)
        {
            var bw = boneWeights[i];

            //the weight is sorted by decending order, so we can get the first two to lower the accuracy and save Computing power.
            //if (bw.weight2 > 0.02f)
            //{
            //    Debug.Log(new Vector4(bw.weight0, bw.weight1, bw.weight2, bw.weight3));
            //}

            indexUV.Add(new Vector2(bw.boneIndex0, bw.boneIndex1));
            weightUV.Add(new Vector2(bw.weight0, bw.weight1));
        }

        mesh.SetUVs((int)weightChannel, weightUV);
        mesh.SetUVs((int)indexChannel, indexUV);
        return true;
    }
}

public enum UVChannel
{
    UV,
    UV2,
    UV3,
    UV4,
    UV5,
    UV6,
    UV7,
    UV8
}