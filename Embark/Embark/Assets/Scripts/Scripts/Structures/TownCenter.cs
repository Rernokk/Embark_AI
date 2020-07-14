using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCenter : StructureBase {
	#region Variables
	[SerializeField]
	float townRadius = 60f;
	#endregion

	#region Properties
	public float TownRadius {
		get {
			return townRadius;
		}

		set {
			townRadius = value;
		}
	}
	#endregion

	void Start () {
		Reposition();
		CivilizationManager.instance.AssignTownHall(this);
	}

	protected override void Reposition()
	{
		base.Reposition();
		if (positionedCorrectly)
		{
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					Debug.DrawRay(transform.position, new Vector3(i, 0, j).normalized * townRadius, Color.red, 10f);
				}
			}
		}
	}
}
