using UnityEngine;

public class DungeonMakeTest : MonoBehaviour
{
	bool ExeAtOnce;
	void FixedUpdate ()
	{
		if (!ExeAtOnce)
		{
			Make ();
			ExeAtOnce = true;
		}
	}
	void Make ()
	{
		GetComponent<DungeonMaker> ().MakeDungeon ();
	}
}