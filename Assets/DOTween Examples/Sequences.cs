using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Sequences : MonoBehaviour
{
	public Transform cubepurple;
	public Transform cubered;
	public Transform cubegreen;

	public float duration = 4;
	private LogInfo logInfo;
	Sequence s;

	IEnumerator Start()
	{
		// Start after one second delay (to ignore Unity hiccups when activating Play mode in Editor)
		yield return new WaitForSeconds(1);
		logInfo = GetComponent<LogInfo>();
		// Create a new Sequence.
		// We will set it so that the whole duration is 6
		s = DOTween.Sequence();
		// Add an horizontal relative move tween that will last the whole Sequence's duration
		s.Append(cubepurple.DOMoveX(6, duration).SetRelative().SetEase(Ease.InOutQuad));
		// Insert a rotation tween which will last half the duration
		// and will loop forward and backward twice
		s.Insert(0, cubered.DORotate(new Vector3(0, 45, 0), duration / 2).SetEase(Ease.InQuad).SetLoops(2, LoopType.Yoyo));
		// Add a color tween that will start at half the duration and last until the end
		s.Insert(duration / 2, cubegreen.GetComponent<Renderer>().material.DOColor(Color.yellow, duration / 2));
		// Set the whole Sequence to loop infinitely forward and backwards
		s.SetLoops(2, LoopType.Yoyo);

		s.OnPlay(() => { Debug.Log("Play"); logInfo.Log("Play"); });
		s.OnComplete(() => { Debug.Log("Complete"); logInfo.Log("Complete"); });
		s.OnRewind(() => { Debug.Log("Rewind"); logInfo.Log("Rewind"); });
		s.OnStart(() => { Debug.Log("Start"); logInfo.Log("Start"); });
		s.OnPause(() => { Debug.Log("Pause"); logInfo.Log("Pause"); });
		s.OnStepComplete(() => { Debug.Log("Step Complete"); logInfo.Log("Step Complete"); });
		s.OnKill(() => { Debug.Log("Kill"); logInfo.Log("Kill"); });
		//s.OnUpdate(() => { Debug.Log("Update"); });

	}

	public void Pause()
    {
		s.Pause();
    }

	public void Play()
    {
		s.Play();
    }

	public void KIll()
	{
		s.Kill();
	}

	public void Restart()
    {
		s.Restart();
    }

	public void Rewind()
    {
		s.Rewind();
    }
}
