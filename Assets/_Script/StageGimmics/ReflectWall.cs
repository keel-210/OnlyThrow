using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectWall : MonoBehaviour, IReflectable
{
	[SerializeField] LayerMask mask;

	public Vector2 Reflect (Rigidbody2D rb, Vector2 Cast)
	{
		Vector2 ReflectVec = Vector2.zero;
		Vector2 hitUp = Physics2D.Raycast (rb.position, Vector2.up, 100f, mask).point - rb.position;
		Vector2 hitDown = Physics2D.Raycast (rb.position, Vector2.down, 100f, mask).point - rb.position;
		Vector2 hitRight = Physics2D.Raycast (rb.position, Vector2.right, 100f, mask).point - rb.position;
		Vector2 hitLeft = Physics2D.Raycast (rb.position, Vector2.left, 100f, mask).point - rb.position;
		Vector2 contactPoint = new Vector2 (Mathf.Abs (hitRight.x) < Mathf.Abs (hitLeft.x) ? hitRight.x : hitLeft.x, Mathf.Abs (hitUp.y) < Mathf.Abs (hitDown.y) ? hitUp.y : hitDown.y);
		Debug.Log (contactPoint.ToString ("F5"));
		if (Mathf.Abs (contactPoint.x) < Mathf.Abs (contactPoint.y))
		{
			ReflectVec = new Vector2 (-Cast.x, Cast.y);
		}
		else
		{
			ReflectVec = new Vector2 (Cast.x, -Cast.y);
		}
		iTween.ShakePosition (Camera.main.gameObject, new Vector2 (0.2f, 0.2f), 0.1f);
		return ReflectVec;
	}
}