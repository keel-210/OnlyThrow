using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMaker : MonoBehaviour
{
	[SerializeField] Cinemachine.CinemachineVirtualCamera VirtualCamera;
	[SerializeField, TimeAndPrfab]
	List<TimeAndPrefab> Wave = new List<TimeAndPrefab> ();
	List<TimeAndPrefab> DeletePrefabs = new List<TimeAndPrefab> ();
	List<GameObject> WaveEnemys = new List<GameObject> ();
	bool Iswaked;
	float Timer;

	void Update ()
	{
		if (Iswaked)
		{
			foreach (TimeAndPrefab w in Wave)
			{
				if (w.Time > Timer)
				{
					GameObject obj = Instantiate (w.Prefab, w.Pos, Quaternion.identity);
					WaveEnemys.Add (obj);
					DeletePrefabs.Add (w);
				}
			}
			Timer += Time.deltaTime;
			if (DeletePrefabs.Count > 0)
			{
				foreach (TimeAndPrefab tp in DeletePrefabs)
				{
					Wave.Remove (tp);
				}
				DeletePrefabs.Clear ();
			}
			WaveEnemys.RemoveAll (item => item == null);
			if (Wave.Count == 0 && WaveEnemys.Count == 0)
			{
				VirtualCamera.enabled = false;
			}
		}
	}
	void OnTriggerEnter2D (Collider2D collider)
	{
		Iswaked = true;
		VirtualCamera.enabled = true;
		Timer = 0;
	}
}