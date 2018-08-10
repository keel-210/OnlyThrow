using UnityEngine;

public class DamageUITest : MonoBehaviour
{
	[SerializeField] int dam;
	void Update ()
	{
		if (Input.GetButtonDown ("Throw"))
		{
			GetComponent<DamageUI> ().Initialize (Vector3.zero, dam);
		}
	}
}