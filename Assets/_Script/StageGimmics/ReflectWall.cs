using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectWall : MonoBehaviour, IReflectable
{
	[SerializeField] LayerMask mask;

	public Vector2 Reflect (Rigidbody2D rb, Vector2 Cast)
	{
		Vector2 ReflectVec = Vector2.zero;
		RaycastHit2D hitUp = Physics2D.Raycast (rb.position, Vector2.up, 100f, mask);
		RaycastHit2D hitDown = Physics2D.Raycast (rb.position, Vector2.down, 100f, mask);
		RaycastHit2D hitRight = Physics2D.Raycast (rb.position, Vector2.right, 100f, mask);
		RaycastHit2D hitLeft = Physics2D.Raycast (rb.position, Vector2.left, 100f, mask);
		if (hitUp && hitDown && hitRight && hitLeft)
		{
			Vector2 up, down, right, left;
			up = hitUp.point - rb.position;
			down = hitDown.point - rb.position;
			right = hitRight.point - rb.position;
			left = hitLeft.point - rb.position;
			Vector2 contactPoint = new Vector2 (Mathf.Abs (right.x) < Mathf.Abs (left.x) ? right.x : left.x,
				Mathf.Abs (up.y) < Mathf.Abs (down.y) ? up.y : down.y);
			if (Mathf.Abs (contactPoint.x) < Mathf.Abs (contactPoint.y))
			{
				ReflectVec = new Vector2 (-Cast.x, Cast.y);
			}
			else
			{
				ReflectVec = new Vector2 (Cast.x, -Cast.y);
			}
			rb.velocity = ReflectVec;
			iTween.ShakePosition (Camera.main.gameObject, new Vector2 (0.2f, 0.2f), 0.1f);
		}
		else
		{
			Debug.Log ("Reflect Axis Mesurement Error");
			Debug.Log ("Pos" + rb.position);
			Debug.Log ("Pos" + rb.transform.position);
			Debug.Log ("Up" + Physics2D.Raycast (rb.position, Vector2.up, 100f, mask).point.ToString ("F5"));
			Debug.Log ("Down" + Physics2D.Raycast (rb.position, Vector2.down, 100f, mask).point.ToString ("F5"));
			Debug.Log ("Right" + Physics2D.Raycast (rb.position, Vector2.right, 100f, mask).point.ToString ("F5"));
			Debug.Log ("Left" + Physics2D.Raycast (rb.position, Vector2.left, 100f, mask).point.ToString ("F5"));
			UnityEditor.EditorApplication.isPaused = true;
		}
		return ReflectVec;
	}
}