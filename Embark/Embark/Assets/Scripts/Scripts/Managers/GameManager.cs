using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	float timeToWave = 1f;

	[SerializeField]
	float waveTimeValue = 15f;

	CivilizationManager civManager;

	public static GameManager instance;

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

	void Start()
	{
		civManager = CivilizationManager.instance;
	}

	void Update()
	{
		if (timeToWave <= 0)
		{
			timeToWave += waveTimeValue;
			civManager.SignalRecruitWave();
		}
		timeToWave -= Time.deltaTime;
	}
}
