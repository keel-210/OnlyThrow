using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//Layers
	//Player = 8
	//PlayerCatchCollider = 25
	//PlayerFloat = 26
	//PlayerRolling = 27
	[SerializeField] float Speed, JumpPower, RollingSpeed, RollingLimitTime;
	[SerializeField] ContactFilter2D OnGroundFilter;
	Rigidbody2D rb;
	List<IThrowable> CatchedObjs = new List<IThrowable> ();
	public bool OnGround, IsRolling;
	float RollingTimer;
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		OnGround = rb.IsTouching (OnGroundFilter);
		if (!IsRolling)
		{
			if (Input.GetAxis ("Horizontal")!= 0)
			{
				Walk ();
			}
			else
			{
				Fall ();
			}
			if (Input.GetButtonDown ("Jump"))
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
		rb.velocity = new Vector2 (rb.velocity.x, JumpPower);
	}
	void Fall ()
	{
		rb.velocity = new Vector2 (0, rb.velocity.y);
	}
	void FallDown ()
	{
		gameObject.layer = 26;
	}
	void Throw ()
	{
		if (CatchedObjs.Count > 0)
		{
			Vector2 MousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Vector2 Dir = (MousePos - rb.position).normalized;
			CatchedObjs[0].Throw (Dir);
			CatchedObjs.Remove (CatchedObjs[0]);
		}
	}
	void Rolling ()
	{
		if (!IsRolling)
		{
			IsRolling = true;
			gameObject.layer = 27;
			RollingTimer = 0;
		}
		RollingTimer += Time.deltaTime;
		rb.velocity = new Vector2 (transform.right.x * RollingSpeed, rb.velocity.y);
		if (RollingTimer > RollingLimitTime)
		{
			IsRolling = false;
		}
	}
	void OnGroundProcess ()
	{
		gameObject.layer = 8;
	}
	void OnTriggerEnter2D (Collider2D obj)
	{
		IThrowable it = obj.GetComponent<IThrowable> ();
		if (it != null)
		{
			it.Catch (transform);
			CatchedObjs.Add (it);
		}
	}
}