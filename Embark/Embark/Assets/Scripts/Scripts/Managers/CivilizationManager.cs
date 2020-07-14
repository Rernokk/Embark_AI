using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationManager : MonoBehaviour
{
	#region Variables
	List<BaseCitizen> citizenList = new List<BaseCitizen>();
	Transform citizenHost;

	[SerializeField]
	GameObject basicCitizen;

	[SerializeField]
	int citizenLimit = 100;

	public static CivilizationManager instance;
	TownCenter townHall;

	#endregion

	#region Properties
	public int CivilizationMemberCount
	{
		get
		{
			return citizenList.Count;
		}
	}

	public TownCenter TownHall {
		get {
			return townHall;
		}

		set {
			townHall = value;
		}
	}

	public List<BaseCitizen> CitizenList {
		get {
			return citizenList;
		}
		
		set {
			citizenList = value;
		}
	}
	#endregion

	#region Private Methods
	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this);
		}
		else
		{
			instance = this;
		}
	}

	#endregion

	#region Public Methods
	public void SignalRecruitWave()
	{
		print("Signal Received");
		if (citizenHost == null)
		{
			citizenHost = new GameObject("CitizenHost").transform;
		}

		for (int i = 0; i < 3; i++)
		{
			if (citizenList.Count < citizenLimit)
			{
				GameObject newMember = Instantiate(basicCitizen, PathManager.instance.WorldGrid[Random.Range(0, WorldManager.instance.WorldSize), Random.Range(0, WorldManager.instance.WorldSize)].transform.position, Quaternion.identity);
				newMember.transform.parent = citizenHost;
			}
		}
	}

	public void AddCitizen(BaseCitizen citizen)
	{
		if (citizenList == null){
			citizenList = new List<BaseCitizen>();
		}

		if (!citizenList.Contains(citizen))
		{
			citizenList.Add(citizen);
		}
	}

	public void AssignTownHall(TownCenter center){
		townHall = center;
	}

	public void TrackCitizen (BaseCitizen citizen){
		Camera.main.GetComponent<CitizenTracking>().TrackingCitizen = citizen;
	}
	#endregion
}
