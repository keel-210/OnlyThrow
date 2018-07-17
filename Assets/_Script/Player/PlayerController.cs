using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float Speed, JumpPower;
	[SerializeField] ContactFilter2D OnGroundFilter;
	Rigidbody2D rb;
	List<IThrowable> CatchedObjs = new List<IThrowable> ();
	public bool OnGround;
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		OnGround = rb.IsTouching (OnGroundFilter);
		if (Input.GetAxis ("Horizontal") != 0)
		{
			rb.velocity = new Vector2 (Input.GetAxisRaw ("Horizontal") * Speed, rb.velocity.y);
		}
		else
		{
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}
		if (Input.GetButtonDown ("Jump") && !(Input.GetAxisRaw ("Vertical") < 0))
		{
			rb.velocity = new Vector2 (rb.velocity.x, JumpPower);
		}
		if (Input.GetButtonDown ("Jump") && Input.GetAxisRaw ("Vertical") < 0)
		{
			Debug.Log ("Fall");
		}
		if (Input.GetButtonDown ("Fire1"))
		{
			if (CatchedObjs.Count > 0)
			{
				Vector2 MousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector2 Dir = (MousePos - rb.position).normalized;
				CatchedObjs[0].Throw (Dir);
				CatchedObjs.Remove (CatchedObjs[0]);
			}
		}
	}
	void FixedUpdate ()
	{

	}
	void OnTriggerEnter2D (Collider2D obj)
	{
		IThrowable it = obj.GetComponent<IThrowable> ();
		if (it != null)
		{
			Debug.Log ("This Object Throwable");
			CatchedObjs.Add (it);
			it.Catch (transform);
		}
	}
}