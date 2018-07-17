using System;
using System.Collections.Generic;
using UnityEngine;

public class TestZakoController : MonoBehaviour, Health, IThrowable
{
	public int health { get; set; }

	[SerializeField] float ThrowedSpeed;
	Rigidbody2D rb;
	Collider2D col;
	public Vector2 velocityCast, CatchedPos;
	bool IsCatched, IsThrowed;
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		col = GetComponent<Collider2D> ();
	}
	void FixedUpdate ()
	{
		if (IsCatched)
		{
			transform.localPosition = CatchedPos;
		}
	}
	public void Catch (Transform player)
	{
		Debug.Log ("Has Catched");
		transform.parent = player;
		CatchedPos = transform.localPosition;
		rb.gravityScale = 0;
		IsCatched = true;
	}
	public void Throw (Vector2 Direction)
	{
		IsCatched = false;
		IsThrowed = true;
		gameObject.layer = 13;
		rb.velocity = Direction * ThrowedSpeed;
		velocityCast = rb.velocity;
		rb.gravityScale = 0;
	}
	void OnCollisionEnter2D (Collision2D obj)
	{
		if (IsThrowed)
		{
			IReflectable IRef = obj.gameObject.GetComponent<IReflectable> ();
			if (IRef != null)
			{
				rb.velocity = IRef.Reflect (rb, velocityCast);
				velocityCast = rb.velocity;
			}
			IDamagable IDam = obj.gameObject.GetComponent<IDamagable> ();
			if (IDam != null)
			{
				IDam.Damage ();
			}
		}
	}
}