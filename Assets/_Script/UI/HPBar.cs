using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
	Transform tra;
	Health health;
	Slider slider;
	Vector3 PosOffset;
	bool TraceTransform;
	public void Initialize (Transform t, Health h, Vector3 v, bool b)
	{
		tra = t;
		health = h;
		PosOffset = v;
		TraceTransform = b;
		slider = GetComponent<Slider> ();
		slider.maxValue = health.health;
		slider.minValue = 0;
		transform.parent = GameObject.Find ("Canvas").transform;
	}
	void Update ()
	{
		if (tra)
		{
			if (TraceTransform)
			{
				transform.position = Camera.main.WorldToScreenPoint (tra.position + PosOffset);
			}
			slider.value = health.health;
		}
		else
		{
			Destroy (gameObject);
		}
	}
}