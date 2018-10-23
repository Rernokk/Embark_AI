using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilizationManager : MonoBehaviour
{
	#region Variables
	List<BaseGatherer> citizenList = new List<BaseGatherer>();
	Transform citizenHost;

	[SerializeField]
	GameObject basicCitizen;

	[SerializeField]
	int citizenLimit = 100;

	public static CivilizationManager instance;
	#endregion

	#region Properties
	public int CivilizationMemberCount
	{
		get
		{
			return citizenList.Count;
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
				citizenList.Add(newMember.GetComponent<BaseGatherer>());
			}
		}
	}

	public void AddCitizen(BaseGatherer citizen)
	{
		if (!citizenList.Contains(citizen))
		{
			citizenList.Add(citizen);
		}
	}
	#endregion
}
