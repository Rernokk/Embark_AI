using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour {
	[SerializeField]
	GameObject CitizenEntry;
	Transform civilianList;

	int citizenCount = 0;
	void Start () {
		civilianList = transform.Find("CitizenList");
	}

	void Update() {
		if (citizenCount != CivilizationManager.instance.CivilizationMemberCount){
			UpdateCitizenListing();
		}
	}

	void UpdateCitizenListing(){
		for (int i = civilianList.childCount-1; i >= 0; i--)
		{
			DestroyImmediate(civilianList.GetChild(i).gameObject);	
		}

		for (int i = 0; i < CivilizationManager.instance.CitizenList.Count; i++)
		{
			GameObject obj = Instantiate(CitizenEntry, civilianList);
			obj.transform.Find("Name").GetComponent<Text>().text = CivilizationManager.instance.CitizenList[i].CitizenName;
			obj.transform.GetComponent<CitizenDataTracker>().CitizenInfo = CivilizationManager.instance.CitizenList[i];
		}
		citizenCount = CivilizationManager.instance.CitizenList.Count;
	}
}
