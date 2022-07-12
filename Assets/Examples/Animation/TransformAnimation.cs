using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum animationType
{
    PunchPosition,
    PunchRotation,
    PunchScale,
    ShakePosition,
    ShakeRotation,
    ShakeScale
}

public class TransformAnimation : MonoBehaviour
{

    public animationType animation;
    public float duration = 1.5f;
    Vector3 position ;
    Vector3 rotation ;
    Vector3 scale ;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
        scale = transform.localScale;

    }

    public void Play()
    {

        switch (animation)
        {
            case animationType.PunchPosition:
                {
                    transform.DOPunchPosition(position + new Vector3(0, 0.2f, 0), duration);
                    break;
                }
            case animationType.PunchRotation:
                {
                    transform.DOPunchRotation(rotation + new Vector3(0, 90, 0), duration);
                    break;
                }
            case animationType.PunchScale:
                {
                    transform.DOPunchScale(scale + new Vector3(2, 2, 2), duration);
                    break;
                }
            case animationType.ShakePosition:
                {
                    transform.DOShakePosition(duration);
                    break;
                }
            case animationType.ShakeRotation:
                {
                    transform.DOShakeRotation(duration);
                    break;
                }
            case animationType.ShakeScale:
                {
                    transform.DOShakeScale(duration);
                    break;
                }
        }

    }
}
