using System.Linq;
using UnityEngine;
public class StageGate : MonoBehaviour
{
	[SerializeField] Cinemachine.CinemachineVirtualCamera VCam;
	void OnTriggerEnter2D (Collider2D obj)
	{
		var Vcams = GameObject.FindGameObjectsWithTag ("VirtualCamera");
		foreach (GameObject g in Vcams)
		{
			g.SetActive (false);
		}
		VCam.gameObject.SetActive (true);
		VCam.m_Lens.OrthographicSize = 5;
		FindObjectOfType<Fader> ().FadeInOut (0.5f);
	}
}