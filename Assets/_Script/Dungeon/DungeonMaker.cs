using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonMaker : MonoBehaviour
{
	public List<GameObject> FreeStages = new List<GameObject> ();
	public List<GameObject> OnlyOneStages = new List<GameObject> ();
	public GameObject StartStage;
	public int StageCount;
	public LayerMask mask;
	bool CannotMakeDungeon;
	public void MakeDungeon ()
	{
		//Select Stages
		int FreeStageCount = StageCount - OnlyOneStages.Count;
		FreeStageCount = FreeStageCount < 0 ? 0 : FreeStageCount;
		List<GameObject> Stages = new List<GameObject> ();
		for (int i = 0; i < FreeStageCount; i++)
		{
			Stages.Add (FreeStages[Random.Range (0, FreeStages.Count)]);
		}
		foreach (GameObject g in OnlyOneStages)
		{
			Stages.Add (g);
		}
		//Instantiate Stages
		GameObject startObj = Instantiate (StartStage, Vector3.zero, Quaternion.identity);
		DungeonStage startObjStage = startObj.GetComponent<DungeonStage> ();
		float width = startObjStage.spriteRenderer.bounds.size.x;
		float height = startObjStage.spriteRenderer.bounds.size.y;
		List<DungeonStage> DungeonStages = new List<DungeonStage> ();
		DungeonStages.Add (startObjStage);
		startObjStage.IsFixed = true;
		foreach (GameObject s in Stages)
		{
			GameObject obj = Instantiate (s, Vector3.zero, Quaternion.identity);
			DungeonStages.Add (obj.GetComponent<DungeonStage> ());
		}
		//Place Stages
		foreach (DungeonStage s in DungeonStages)
		{
			if (!s.IsFixed)
			{
				List<DungeonStage> PlaceRates = DungeonStages.Where (item => item.IsFixed == true).ToList ();
				foreach (DungeonStage d in PlaceRates)
				{
					d.UpGenerateRate = d.UpGenerateRate * Random.value;
					d.DownGenerateRate = d.DownGenerateRate * Random.value;
					d.RightGenerateRate = d.RightGenerateRate * Random.value;
					d.LeftGenerateRate = d.LeftGenerateRate * Random.value;
				}
				PlaceStage (s, PlaceRates);
				FixRates (PlaceRates);
			}
		}
		if (CannotMakeDungeon)
		{
			CannotMakeDungeon = false;
			MakeDungeon ();
		}
		else
		{
			DungeonStages.ForEach (item => Destroy (item.GetComponent<Collider> ()));
		}
	}
	void PlaceStage (DungeonStage Place, List<DungeonStage> list)
	{
		List<DungeonStage> SortedRate = list.OrderByDescending (item => Mathf.Max (item.UpGenerateRate, item.DownGenerateRate, item.RightGenerateRate, item.LeftGenerateRate)).ToList ();
		float MaxRate = 0;
		DungeonStage Fixed = null;

		for (int i = 0; CollideCheck (Place)&& i < SortedRate.Count; i++)
		{
			Fixed = SortedRate[i];
			MaxRate = Mathf.Max (Fixed.UpGenerateRate, Fixed.DownGenerateRate, Fixed.RightGenerateRate, Fixed.LeftGenerateRate);
			float PWidth = Place.spriteRenderer.bounds.extents.x;
			float PHeight = Place.spriteRenderer.bounds.extents.y;
			float FWidth = Fixed.spriteRenderer.bounds.extents.x;
			float FHeight = Fixed.spriteRenderer.bounds.extents.y;
			if (MaxRate == Fixed.UpGenerateRate)
			{
				Place.transform.position = Fixed.transform.position +
					new Vector3 (Place.UpGate.localPosition.x + Fixed.UpGate.localPosition.x, PHeight + FHeight, 0);
			}
			if (MaxRate == Fixed.DownGenerateRate)
			{
				Place.transform.position = Fixed.transform.position +
					new Vector3 (Place.DownGate.localPosition.x + Fixed.DownGate.localPosition.x, -PHeight - FHeight, 0);
			}
			if (MaxRate == Fixed.RightGenerateRate)
			{
				Place.transform.position = Fixed.transform.position +
					new Vector3 (PWidth + FWidth, Place.RightGate.localPosition.y + Fixed.RightGate.localPosition.y, 0);
			}
			if (MaxRate == Fixed.LeftGenerateRate)
			{
				Place.transform.position = Fixed.transform.position +
					new Vector3 (-PWidth - FWidth, Place.LeftGate.localPosition.y + Fixed.LeftGate.localPosition.y, 0);
			}
			if (0 < i && i == SortedRate.Count - 1 && CollideCheck (Place))
			{
				CannotMakeDungeon = true;
				Debug.Log ("Remake Dungeon" + i);

			}
		}
		if (MaxRate == Fixed.UpGenerateRate)
		{
			Place.DownGenerateRate = 0;
			Place.DownGate.gameObject.SetActive (true);
			Fixed.UpGenerateRate = 0;
			Fixed.UpGate.gameObject.SetActive (true);
		}
		if (MaxRate == Fixed.DownGenerateRate)
		{
			Place.UpGenerateRate = 0;
			Place.UpGate.gameObject.SetActive (true);
			Fixed.DownGenerateRate = 0;
			Fixed.DownGate.gameObject.SetActive (true);
		}
		if (MaxRate == Fixed.RightGenerateRate)
		{
			Place.LeftGenerateRate = 0;
			Place.LeftGate.gameObject.SetActive (true);
			Fixed.RightGenerateRate = 0;
			Fixed.RightGate.gameObject.SetActive (true);
		}
		if (MaxRate == Fixed.LeftGenerateRate)
		{
			Place.RightGenerateRate = 0;
			Place.RightGate.gameObject.SetActive (true);
			Fixed.LeftGenerateRate = 0;
			Fixed.LeftGate.gameObject.SetActive (true);
		}
		Place.IsFixed = true;
	}
	void FixRates (List<DungeonStage> list)
	{
		foreach (DungeonStage d in list)
		{
			if (d.UpGenerateRate != 0)
				d.UpGenerateRate = 1;
			if (d.DownGenerateRate != 0)
				d.DownGenerateRate = 1;
			if (d.RightGenerateRate != 0)
				d.RightGenerateRate = 1;
			if (d.LeftGenerateRate != 0)
				d.LeftGenerateRate = 1;
		}
	}
	bool CollideCheck (DungeonStage Place)
	{
		Place.gameObject.layer = 2;
		float width = Place.spriteRenderer.bounds.extents.x;
		float height = Place.spriteRenderer.bounds.extents.y;
		Vector3 pos = Place.transform.position + new Vector3 (0, 0, -10f);
		Ray PosRay = new Ray (pos, Vector3.forward);
		Ray UpLeftRay = new Ray (pos + new Vector3 (-width, height, 0)- new Vector3 (-0.01f, 0.01f, 0), Vector3.forward);
		Ray DownLeftRay = new Ray (pos + new Vector3 (-width, -height, 0)- new Vector3 (-0.01f, -0.01f, 0), Vector3.forward);
		Ray UpRightRay = new Ray (pos + new Vector3 (width, height, 0)- new Vector3 (0.01f, 0.01f, 0), Vector3.forward);
		Ray DownRightRay = new Ray (pos + new Vector3 (width, -height, 0)- new Vector3 (0.01f, -0.01f, 0), Vector3.forward);
		bool Pos = Physics.Raycast (PosRay, 100f, mask);
		bool UpLeft = Physics.Raycast (UpLeftRay, 100f, mask);
		bool DownLeft = Physics.Raycast (DownLeftRay, 100f, mask);
		bool UpRight = Physics.Raycast (UpRightRay, 100f, mask);
		bool DownRight = Physics.Raycast (DownRightRay, 100f, mask);
		Place.gameObject.layer = 15;
		return Pos || UpLeft || UpRight || DownLeft || DownRight;
	}
}