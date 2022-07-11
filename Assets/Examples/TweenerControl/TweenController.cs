using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class TweenController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<IdentifiableTweener> tweenerConfigues;
    [SerializeField] TextMeshProUGUI TotalPlayingTweeningCount;
    [SerializeField] TextMeshProUGUI TotalActiveTweeningCount;
    [SerializeField] Transform ContentList;
    [SerializeField] TMP_InputField input;
    [SerializeField] GameObject tweenerItemPrefab;

    void Start()
    {
        DOTween.defaultAutoKill = false;

        for (int i = 0; i < tweenerConfigues.Count; i++)
        {
            // create UI
            GameObject tweenerItem = Instantiate(tweenerItemPrefab, ContentList);
            RectTransform target = tweenerItem.GetComponent<TweenerView>().target;
            //create tweens

            IdentifiableTweener identifiableTweener = tweenerConfigues[i];
            int loopCount = identifiableTweener.loopCount;
            string id = identifiableTweener.id;
            Vector3 position = target.anchoredPosition;
            float duration = identifiableTweener.duration;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(target.DOAnchorPosX(position.x + 300, duration));
            sequence.Append(target.transform.DOLocalRotate(new Vector3(0, 0, 90), duration).SetLoops(2, LoopType.Yoyo));
            sequence.Append(target.transform.DOPunchScale(Vector3.one * 2, duration * 2));
            sequence.SetAutoKill(false);
            Tween tween = sequence;
            tween.SetId(id);
            tween.SetLoops(loopCount);

            //register events and datas
            tweenerItem.GetComponent<TweenerView>().tween = tween;
        }
    }

    #region UIMethods
    public void PlayTween()
    {
        string id = input.text;
        DOTween.Play(id);
    }

    public void PauseTween()
    {
        string id = input.text;
        DOTween.Pause(id);
    }
    public void RestartTween()
    {
        string id = input.text;
        DOTween.Restart(id);
    }

    public void KillTween()
    {
        string id = input.text;
        DOTween.Kill(id);
    }

    public void PlayAll()
    {
        DOTween.PlayAll();
    }

    public void RestartAll()
    {
        DOTween.RestartAll();
    }

    public void PauseAll()
    {
        DOTween.PauseAll();
    }

    public void KillAll()
    {
        DOTween.KillAll();
    }

    #endregion
    
    private void Update()
    {
        TotalActiveTweeningCount.text = "Total Active Tweening: " + DOTween.TotalActiveTweens().ToString();
        TotalPlayingTweeningCount.text = "Total Playing Tweening: " + DOTween.TotalPlayingTweens().ToString();
    }
}

[Serializable]
public class IdentifiableTweener
{
    public string id;
    public float duration = 1;
    public int loopCount = 1;
}
