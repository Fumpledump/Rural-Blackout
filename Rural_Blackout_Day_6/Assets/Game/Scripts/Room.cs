using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	public bool LightSwitchOn => lightSwitch.IsEmitting;

	[SerializeField] 
	public BreakInPoint[] breakInPoints;

	public LightSwitch lightSwitch;

	BreakInPoint activeBreakInPoint;

	void Awake()
	{
		lightSwitch.OnStartedEmitting += OnLightSwitchedOn;
	}

	void OnDestroy()
	{
		lightSwitch.OnStartedEmitting -= OnLightSwitchedOn;
	}

	void OnLightSwitchedOn()
	{
		if(activeBreakInPoint)
		{
			activeBreakInPoint.StopBreakIn();
		}
	}

	public void StartBreakIn()
	{
		activeBreakInPoint = GetRandomBreakInPoint();
		activeBreakInPoint.StartBreakIn();
	}

	BreakInPoint GetRandomBreakInPoint()
	{
		return breakInPoints[Random.Range(0, breakInPoints.Length)];
	}
}
