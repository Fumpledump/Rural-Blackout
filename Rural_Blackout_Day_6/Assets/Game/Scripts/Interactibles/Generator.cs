using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Interactable
{
	public static float FuelPercent { get; set; }
	public static bool HasPower => FuelPercent > 0;

	public static event Action OnRanOutOfFuel;
	public static event Action OnRefueled;

	[SerializeField] GameObject idleFuelCan;
	[SerializeField] GameObject fillingFuelCan;
	[SerializeField] float fuelDecreaseSpeed = 0.1f;
	[SerializeField] float refuelSpeed = 0.1f;
	[SerializeField] Light statusLight;
	[SerializeField] AudioSource generatorSound;
	[SerializeField] AudioSource refuelingSound;
	[SerializeField] Color fullColor;
	[SerializeField] Color lowColor;
	[SerializeField] Color emptyColor;

	enum GeneratorState { Off, Refueling, On };
	GeneratorState state;

	void Start()
	{
		FuelPercent = 0f;
		SetState(GeneratorState.Off);
	}

	void Update()
	{
		switch(state)
		{
			case GeneratorState.Off:
				statusLight.color = emptyColor;
				break;
			case GeneratorState.Refueling:
				FuelPercent += refuelSpeed * Time.deltaTime;

				SetPromptText($"{(FuelPercent * 100f).ToString("0")}% FUELED");

				if(FuelPercent >= 1f)
				{
					FuelPercent = 1f;
					SetState(GeneratorState.On);
				}
				break;
			case GeneratorState.On:

				FuelPercent -= (fuelDecreaseSpeed * (LightSwitch.EmitCount + 1)) * Time.deltaTime;

				if(FuelPercent <= 0f)
				{
					OnRanOutOfFuel?.Invoke();
					SetState(GeneratorState.Off);
				}
				break;
		}

		UpdateLightColor();
	}

	void SetState(GeneratorState newState)
	{
		state = newState;

		switch(newState)
		{
			case GeneratorState.Off:
				statusLight.color = emptyColor;
				generatorSound.Stop();
				refuelingSound.Stop();
				statusLight.enabled = true;
				fillingFuelCan.SetActive(false);
				idleFuelCan.SetActive(true);
				//SetPromptText($"HOLD E");
				SetPromptText(promptText);
				break;
			case GeneratorState.Refueling:
				generatorSound.Stop();
				refuelingSound.Play();
				statusLight.enabled = false;
				SetPromptText($"0% FUELED");
				idleFuelCan.SetActive(false);
				fillingFuelCan.SetActive(true);
				break;
			case GeneratorState.On:
				generatorSound.Play();
				refuelingSound.Stop();
				statusLight.enabled = true;
				fillingFuelCan.SetActive(false);
				idleFuelCan.SetActive(true);
				SetPromptText($"HOLD E");
				//SetPromptText(promptText);
				break;
		}
	}

	void UpdateLightColor()
	{
		if(FuelPercent >= 0.6f)
		{
			statusLight.color = fullColor;
		}
		else if(FuelPercent > 0f)
		{
			statusLight.color = lowColor;
		}
		else if(FuelPercent <= 0f)
		{
			statusLight.color = emptyColor;
		}
	}

	public override void OnPress()
	{
		SetState(GeneratorState.Refueling);
	}

	public override void OnRelease()
	{
		if(state != GeneratorState.On)
		{
			SetState(GeneratorState.On);
		}
		OnRefueled?.Invoke();
	}
}
