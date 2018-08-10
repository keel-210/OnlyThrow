using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamColliderFitter : MonoBehaviour
{
	[SerializeField] Camera TargetCam;
	void Update ()
	{
		var worldHeight = TargetCam.orthographicSize * 2f;
		var worldWidth = worldHeight / Screen.height * Screen.width;
		transform.localScale = new Vector3 (worldWidth, worldHeight);

		var tempPosition = Camera.main.transform.position;
		tempPosition.z = 0f;
		transform.position = tempPosition;
	}
}