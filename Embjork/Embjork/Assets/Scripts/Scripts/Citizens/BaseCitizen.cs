using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCitizen : MonoBehaviour
{
	#region Variable
	[SerializeField]
	protected float unitSpeed = 5f;
	protected List<Vector3> currentPath;

	[SerializeField]
	protected string citizenName;

	[SerializeField]
	private float walkTimer = 5f;
	#endregion

	#region Properties
	public string CitizenName {
		get {
			return citizenName;
		}

		set {
			citizenName = value;
		}
	}
	#endregion

	protected virtual void Start()
	{
		CivilizationManager.instance.AddCitizen(this);
		currentPath = new List<Vector3>();
		walkTimer = Random.Range(1f, 5f);

		citizenName = "";
		for (int i = 0; i < Random.Range(6, 12); i++){
			citizenName += (char)Random.Range(97, 123);
			if (i == 0){
				citizenName = citizenName.ToUpper();
			}
		}
	}

	protected virtual void Update()
	{
		BasicWander();
	}

	protected void BasicWander()
	{
		if (currentPath.Count > 0)
		{
			//Traverse Path
			transform.position += (currentPath[0] - transform.position).normalized * unitSpeed * Time.deltaTime;
			if (Vector3.Distance(transform.position, currentPath[0]) < unitSpeed * Time.deltaTime)
			{
				currentPath.RemoveAt(0);
			}
		}
		else
		{
			if (walkTimer > 0)
			{
				//Tick Down
				walkTimer -= Time.deltaTime;
			}
			else
			{
				//Reset Timer
				walkTimer = 5f;

				//Target Position
				TownCenter THRef = CivilizationManager.instance.TownHall;
				Vector3 targetPos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
				targetPos = targetPos.normalized * THRef.TownRadius;
				targetPos += THRef.transform.position;


				//Fetch Path
				currentPath = PathManager.instance.RunPathfinder(transform.position, targetPos);
			}
		}
	}
}
