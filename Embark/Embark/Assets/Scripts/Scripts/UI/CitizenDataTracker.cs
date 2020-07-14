using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenDataTracker : MonoBehaviour {
	BaseCitizen myCitizenInfo;

	public BaseCitizen CitizenInfo {
		get {
			return myCitizenInfo;
		}

		set {
			myCitizenInfo = value;
		}
	}
	
	public void AssignTrack() {
		CivilizationManager.instance.TrackCitizen(myCitizenInfo);
	}
}
