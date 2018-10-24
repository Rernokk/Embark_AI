using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenTracking : MonoBehaviour
{
	[SerializeField]
	BaseCitizen trackingCitizen;

	#region Property
	public BaseCitizen TrackingCitizen {
		get {
			return trackingCitizen;
		}

		set {
			trackingCitizen = value;
		}
	}
	#endregion

	// Use this for initialization
	void Start()
	{
		trackingCitizen = null;
	}

	// Update is called once per frame
	void Update()
	{
		if (trackingCitizen != null)
		{
			transform.parent.position = Vector3.Lerp(transform.parent.position, trackingCitizen.transform.position, 5f * Time.deltaTime);
		}
	}
}
