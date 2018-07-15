using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float Speed;
	Rigidbody2D rb;
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		if (Input.GetAxis ("Horizontal") != 0)
		{
			rb.velocity = new Vector2 (Input.GetAxisRaw ("Horizontal") * Speed, rb.velocity.y);
		}
		else
		{
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}
	}
	void FixedUpdate ()
	{

	}
}