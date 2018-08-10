using UnityEngine;

[System.Serializable]
public class TimeAndPrefab
{
	public float Time;
	public GameObject Prefab;
	public Vector3 Pos;

}
public class TimeAndPrfabAttribute : PropertyAttribute { }