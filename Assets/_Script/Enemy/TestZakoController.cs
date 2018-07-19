using System;
using System.Collections.Generic;
using UnityEngine;

public class TestZakoController : MonoBehaviour, Health, IThrowable, IHitEnemy, IDamagable
{
	[SerializeField] int Health;
	public int health { get { return Health; } set { Health = value; } }

	[SerializeField] float ThrowedSpeed;
	[SerializeField] Vector2 HitVelocity;
	Rigidbody2D rb;
	Collider2D col;
	Vector2 velocityCast, CatchedPos;
	bool IsCatched, IsThrowed;
	int ReflectedCount;
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
		rb.velocity = Vector2.zero;
		GetComponent<Collider2D> ().isTrigger = true;
		IsCatched = true;
	}
	public void Throw (Vector2 Direction)
	{
		IsCatched = false;
		IsThrowed = true;
		gameObject.layer = 13;
		transform.parent = null;
		rb.velocity = Direction * ThrowedSpeed;
		velocityCast = rb.velocity;
		rb.gravityScale = 0;
		GetComponent<Collider2D> ().isTrigger = false;
		ReflectedCount = 0;
	}
	public void HitEnemy ()
	{

	}
	public void Damage ()
	{

	}
	void OnCollisionEnter2D (Collision2D obj)
	{
		if (IsThrowed)
		{
			IReflectable IRef = obj.gameObject.GetComponent<IReflectable> ();
			if (IRef != null)
			{
				IRef.Reflect (rb, velocityCast);
				velocityCast = rb.velocity;
				ReflectedCount++;
			}
			IDamagable IDam = obj.gameObject.GetComponent<IDamagable> ();
			if (IDam != null)
			{
				IDam.Damage ();
			}
			IHitEnemy IHit = obj.gameObject.GetComponent<IHitEnemy> ();
			if (IHit != null)
			{
				IHit.HitEnemy ();
			}
		}
	}
}