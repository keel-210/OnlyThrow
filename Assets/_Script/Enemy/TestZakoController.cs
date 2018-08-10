using System;
using System.Collections.Generic;
using UnityEngine;

public class TestZakoController : MonoBehaviour, Health, IThrowable, IHitEnemy, IDamagable
{
	public int health { get { return Health; } set { Health = value; } }

	[SerializeField] int Health;
	[SerializeField] float ThrowedSpeed;
	[SerializeField] Vector2 HitVelocity;
	[SerializeField] GameObject Bar, DamageObj;
	[SerializeField] List<GameObject> OnDestroyItems;
	Rigidbody2D rb;
	Collider2D col;
	public Vector2 velocityCast, CatchedPos;
	EnemyState state = EnemyState.Idle;
	int ReflectedCount;
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		col = GetComponent<Collider2D> ();
		GameObject bar = Instantiate (Bar);
		bar.GetComponent<HPBar> ().Initialize (transform, GetComponent<Health> (), Vector3.up, true);
	}
	void FixedUpdate ()
	{
		if (state == EnemyState.Catched)
		{
			transform.localPosition = CatchedPos;
		}
		if (health <= 0)
		{
			Death ();
		}
	}
	public IThrowable Catch (Transform player)
	{
		transform.parent = player;
		CatchedPos = transform.localPosition;
		rb.gravityScale = 0;
		rb.velocity = Vector2.zero;
		GetComponent<Collider2D> ().isTrigger = true;
		state = EnemyState.Catched;
		return GetComponent<IThrowable> ();
	}
	public void Throw (Vector2 Direction)
	{
		state = EnemyState.Throwed;
		if (gameObject)
		{
			gameObject.layer = 13;
			transform.parent = GameObject.Find ("Enemys").transform;
			rb.velocity = Direction * ThrowedSpeed;
			velocityCast = rb.velocity;
			rb.gravityScale = 0;
			GetComponent<Collider2D> ().isTrigger = false;
			ReflectedCount = 0;
		}
	}
	public void HitEnemy (Vector2 hitDir, int damage)
	{
		if (state != EnemyState.Hit)
		{
			state = EnemyState.Hit;
			gameObject.layer = 14;
			rb.velocity = HitVelocity;
			health -= damage;
		}
		StartCoroutine (this.DelayMethod (0.5f, ()=>
		{
			if (state == EnemyState.Hit)
			{
				state = EnemyState.Idle;
				gameObject.layer = 9;
				rb.velocity = Vector2.zero;
			}
		}));
	}
	public void Damage (int damage)
	{
		health -= damage;
		GameObject obj = Instantiate (DamageObj, transform.position, Quaternion.identity);
		obj.GetComponent<DamageUI> ().Initialize (transform.position, damage);
	}
	void Death ()
	{
		state = EnemyState.Death;
		foreach (GameObject g in OnDestroyItems)
		{
			Instantiate (g, transform.position, Quaternion.identity);
		}
		Destroy (gameObject);
	}
	void OnCollisionEnter2D (Collision2D obj)
	{
		if (state == EnemyState.Throwed)
		{
			IReflectable[] IRef = obj.gameObject.GetComponents<IReflectable> ();
			foreach (IReflectable i in IRef)
			{
				i.Reflect (rb, velocityCast);
				velocityCast = rb.velocity;
				ReflectedCount++;
			}
			IDamagable IDam = obj.gameObject.GetComponent<IDamagable> ();
			if (IDam != null)
			{
				IDam.Damage (10);
			}
			IHitEnemy IHit = obj.gameObject.GetComponent<IHitEnemy> ();
			if (IHit != null)
			{
				IHit.HitEnemy (transform.position - obj.transform.position, 5);
			}
		}
	}

}