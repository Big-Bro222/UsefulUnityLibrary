using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class EaseMode : MonoBehaviour
{
    [SerializeField] private RectTransform ui;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    private float offset = 200;
    public void SetEase( Ease EaseType)
    {
        float x = ui.anchoredPosition.x;
        ui.DOAnchorPosX(x+offset,2f).SetLoops(-1).SetEase(EaseType);
        textMeshProUGUI.text = EaseType.ToString();
    }
}
