using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class DamageUI : MonoBehaviour
{
	[SerializeField] GameObject Num;
	[SerializeField] float InitTime, AlphaOffset;
	Vector3 UIPos, PosOffset;
	CanvasGroup cg;
	char[] n;
	bool IsFading;
	List<AnimFinished4Animator> Isfinisheds = new List<AnimFinished4Animator> ();
	public void Initialize (Vector3 pos, int damage)
	{
		cg = GetComponent<CanvasGroup> ();
		transform.parent = GameObject.Find ("Canvas").transform;
		transform.position = Camera.main.WorldToScreenPoint (pos);
		transform.localScale = Vector3.one;
		n = damage.ToString ().ToCharArray ();
		PosOffset = new Vector3 (Num.GetComponent<RectTransform> ().sizeDelta.x, 0, 0);
		StartCoroutine (DelayInstantiate (InitTime));
	}
	void Update ()
	{
		if (IsFading)
		{
			cg.alpha -= AlphaOffset * Time.deltaTime;
			if (cg.alpha == 0)
			{
				Destroy (gameObject);
			}
		}
	}
	IEnumerator DelayInstantiate (float WaitTime)
	{
		float i = 0;
		foreach (char c in n)
		{
			GameObject g = Instantiate (Num);
			g.transform.parent = transform;
			g.transform.localPosition = PosOffset * i;
			g.transform.localScale = Vector3.one;
			Isfinisheds.Add (g.GetComponent<AnimFinished4Animator> ());
			g.GetComponent<Text> ().text = c.ToString ();
			yield return new WaitForSeconds (WaitTime);
			i++;
		}
		yield return new WaitForSeconds (0.05f);
		while (!Isfinisheds.All (f => f.IsFinished))
		{
			yield return new WaitForSeconds (0.05f);
		};
		IsFading = true;
	}
}