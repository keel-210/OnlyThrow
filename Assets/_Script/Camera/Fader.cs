using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Fader : MonoBehaviour
{
	IFade fade;

	void Start ()
	{
		Init ();
		fade.Range = cutoutRange;
	}

	float cutoutRange;

	void Init ()
	{
		fade = GetComponent<IFade> ();
	}

	void OnValidate ()
	{
		Init ();
		fade.Range = cutoutRange;
	}
	public void FadeInOut (float fadeTime)
	{
		StartCoroutine (FadeinCoroutine (fadeTime, ()=>
		{
			StartCoroutine (FadeoutCoroutine (fadeTime, ()=> { }));
		}));
	}
	IEnumerator FadeoutCoroutine (float time, System.Action action)
	{
		float endTime = Time.timeSinceLevelLoad + time * (cutoutRange);

		var endFrame = new WaitForEndOfFrame ();

		while (Time.timeSinceLevelLoad <= endTime)
		{
			cutoutRange = (endTime - Time.timeSinceLevelLoad)/ time;
			fade.Range = cutoutRange;
			yield return endFrame;
		}
		cutoutRange = 0;
		fade.Range = cutoutRange;

		if (action != null)
		{
			action ();
		}
	}

	IEnumerator FadeinCoroutine (float time, System.Action action)
	{
		float endTime = Time.timeSinceLevelLoad + time * (1 - cutoutRange);

		var endFrame = new WaitForEndOfFrame ();

		while (Time.timeSinceLevelLoad <= endTime)
		{
			cutoutRange = 1 - ((endTime - Time.timeSinceLevelLoad)/ time);
			fade.Range = cutoutRange;
			yield return endFrame;
		}
		cutoutRange = 1;
		fade.Range = cutoutRange;

		if (action != null)
		{
			action ();
		}
	}
}