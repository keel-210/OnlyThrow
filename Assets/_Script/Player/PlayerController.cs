using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Health, IDamagable
{
	//Layers
	//Player = 8
	//PlayerCatchCollider = 25
	//PlayerFloat = 26
	//PlayerRolling = 27
	[SerializeField] float Speed, JumpPower, RollingSpeed, RollingLimitTime, ShortJumpLimit;
	[SerializeField] ContactFilter2D OnGroundFilter;
	[SerializeField] HPBar bar;
	Rigidbody2D rb;
	List<IThrowable> CatchedObjs = new List<IThrowable> ();
	public bool OnGround;
	int HasJumpedCount;
	float JumpTimer;
	public int health { get { return Health; } set { Health = value; } }

	[SerializeField] int Health;
	PlayerState state = PlayerState.Idle;
	PlayerParamater pp;
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		pp = GetComponent<PlayerParamater> ();
		bar.Initialize (transform, GetComponent<Health> (), Vector3.zero, false);
	}
	void Update ()
	{
		OnGround = rb.IsTouching (OnGroundFilter);
		if (state != PlayerState.Rolling)
		{
			if (Input.GetAxis ("Horizontal")!= 0)
			{
				Walk ();
			}
			else
			{
				Fall ();
			}
			if (Input.GetButton ("Jump"))
			{
				if (!(Input.GetAxisRaw ("Vertical")< 0))
				{
					Jump ();
				}
				else
				{
					FallDown ();
				}
			}
			if (Input.GetButtonDown ("Roll"))
			{
				Rolling ();
			}
			if (Input.GetButtonDown ("Throw"))
			{
				Throw ();
			}
			if (OnGround)
			{
				OnGroundProcess ();
			}
		}
		else
		{
			Rolling ();
		}
	}
	void Walk ()
	{
		rb.velocity = new Vector2 (Input.GetAxisRaw ("Horizontal")* Speed, rb.velocity.y);
		state = PlayerState.Walk;
		if (Input.GetAxisRaw ("Horizontal")> 0)
		{
			transform.rotation = Quaternion.Euler (0, 0, 0);
		}
		else if (Input.GetAxisRaw ("Horizontal")< 0)
		{
			transform.rotation = Quaternion.Euler (0, 180, 0);
		}
	}
	void Jump ()
	{
		if (OnGround)
		{
			if (Input.GetButtonDown ("Jump"))
			{
				rb.velocity = new Vector2 (rb.velocity.x, JumpPower);
				HasJumpedCount++;
				JumpTimer = 0;
			}
		}
		else if (HasJumpedCount < 2)
		{
			if (Input.GetButtonDown ("Jump"))
			{
				rb.velocity = new Vector2 (rb.velocity.x, JumpPower);
				HasJumpedCount++;
				JumpTimer = 0;
			}
		}
		JumpTimer += Time.deltaTime;
		if (JumpTimer < ShortJumpLimit && HasJumpedCount < 2)
		{
			rb.velocity = new Vector2 (rb.velocity.x, JumpPower);
		}
	}
	void Fall ()
	{
		rb.velocity = new Vector2 (Mathf.Lerp (rb.velocity.x, 0, 0.01f), rb.velocity.y);
		if (OnGround)
		{
			state = PlayerState.Idle;
		}
		else
		{
			state = PlayerState.InAir;
		}
	}
	void FallDown ()
	{
		gameObject.layer = 26;
		OnGround = false;
		StartCoroutine (this.DelayMethod (0.3f, ()=>
		{
			gameObject.layer = 8;
		}));
	}
	void Throw ()
	{
		if (CatchedObjs.Count > 0)
		{
			Vector2 MousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector2 Dir = (MousePos - rb.position).normalized;
			if (CatchedObjs[0] != null)
			{
				CatchedObjs[0].Throw (Dir);
				CatchedObjs.Remove (CatchedObjs[0]);
			}
			CatchedObjs.RemoveAll (item => item == null);
		}
	}
	void Rolling ()
	{
		if (state != PlayerState.Rolling)
		{
			state = PlayerState.Rolling;
			gameObject.layer = 27;
		}
		rb.velocity = new Vector2 (transform.right.x * RollingSpeed, 0);
		StartCoroutine (this.DelayMethod (RollingLimitTime, ()=>
		{
			state = PlayerState.Idle;
			gameObject.layer = 8;
		}));
	}
	void OnGroundProcess ()
	{
		gameObject.layer = 8;
		if (rb.velocity.y <= 0)
		{
			HasJumpedCount = 0;
		}
	}
	public void Damage (int damage)
	{
		health -= damage;
	}
	void OnTriggerEnter2D (Collider2D obj)
	{
		IThrowable it = obj.GetComponent<IThrowable> ();
		if (it != null)
		{
			IThrowable iThrow = it.Catch (transform);
			if (iThrow != null)
			{
				CatchedObjs.Add (it);
			}
		}
	}
}