using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Interactable
{
	[SerializeField] SecuritySystem securitySystem;
	[SerializeField] AudioSource shutdownAudio;

	float interactTime;
	float inputRestrictionTime = 0.2f;

	[SerializeField] bool isBroken;//just means that it sohuld work and give the player a prompt saying its broken(its used in the water level)

	void Awake()
	{
		Generator.OnRanOutOfFuel += Shutdown;
	}

	void OnDestroy()
	{
		Generator.OnRanOutOfFuel -= Shutdown;
	}

	void Shutdown()
	{
		shutdownAudio.Play();
	}

	public override void OnPress()
	{
		if (!isBroken)
		{
		    interactTime = Time.realtimeSinceStartup;
		    securitySystem.Activate();
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.E) && Time.realtimeSinceStartup > interactTime + inputRestrictionTime && !isBroken)
		{
			securitySystem.Deactivate();
		}
	}
}