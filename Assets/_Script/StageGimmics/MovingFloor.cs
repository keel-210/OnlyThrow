using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
	[SerializeField] Vector3 StartPos, EndPos;
	[SerializeField] float MoveTime;
	float MaxVelocity, Timer;
	void Start ()
	{
		transform.position = StartPos;
		MaxVelocity = (StartPos - EndPos).magnitude / MoveTime;
	}

	void FixedUpdate ()
	{
		Timer += Time.deltaTime;
		if (Timer < MoveTime)
		{
			transform.position = Vector3.MoveTowards (transform.position, EndPos, MaxVelocity * Time.deltaTime);
		}
		else if (Timer < MoveTime * 2)
		{
			transform.position = Vector3.MoveTowards (transform.position, StartPos, MaxVelocity * Time.deltaTime);
		}
		else
		{
			Timer = 0;
		}
	}
}