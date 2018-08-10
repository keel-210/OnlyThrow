using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonStage : MonoBehaviour
{
	public bool IsFixed;
	public float UpGenerateRate, DownGenerateRate, RightGenerateRate, LeftGenerateRate;
	public SpriteRenderer spriteRenderer;
	public Transform UpGate, DownGate, RightGate, LeftGate;
	[SerializeField] Cinemachine.CinemachineVirtualCamera VCam;
	void Start ()
	{
		VCam.Follow = GameObject.FindGameObjectWithTag ("Player").transform;
	}
}