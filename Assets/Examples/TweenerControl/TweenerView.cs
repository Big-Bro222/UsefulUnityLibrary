using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TweenerView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI percentage;
    [SerializeField] TextMeshProUGUI isPlaying;
    [SerializeField] TextMeshProUGUI isActive;
    [SerializeField] TextMeshProUGUI Loops;
    [SerializeField] TextMeshProUGUI isCompleted;
    [SerializeField] TextMeshProUGUI id;

    [SerializeField] Slider slider;
    public RectTransform target;
    public Tween tween;

    // Update is called once per frame
    void Update()
    {
        if (tween != null)
        {
            percentage.text = (tween.ElapsedPercentage(false)*100).ToString("F2")+" %";
            isPlaying.color = tween.IsPlaying() ? Color.green : Color.red;
            isActive.color = tween.IsActive() ? Color.green : Color.red;
            isCompleted.color = tween.IsComplete() ? Color.green : Color.red;
            Loops.text = tween.Loops().ToString();
            id.text = tween.stringId;
            slider.value = tween.ElapsedPercentage(false);
        }
    }
}
