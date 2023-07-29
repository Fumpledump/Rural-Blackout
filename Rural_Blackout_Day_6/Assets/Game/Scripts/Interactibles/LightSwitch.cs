using System;
using UnityEngine;

public class LightSwitch : Interactable
{
	public bool IsEmitting { get; private set; }
	public static int EmitCount { get; private set; }
	public bool isSwitchedOn;

	[SerializeField] GameObject lightContainer;

	[SerializeField] Material onMaterial;
	[SerializeField] Material offMaterial;
	[SerializeField] AudioSource lightSwitchAudio;

	public event Action OnStartedEmitting;

	Renderer rend;

	void Awake()
	{
		rend = GetComponent<Renderer>();
		Generator.OnRanOutOfFuel += OnGeneratorRanOutOfFuel;
		Generator.OnRefueled += OnGeneratorRefueled;
		EmitCount = 0;
	}

	void OnDestroy()
	{
		Generator.OnRanOutOfFuel -= OnGeneratorRanOutOfFuel;
		Generator.OnRefueled -= OnGeneratorRefueled;
	}

	void Start()
	{
		SetEmitting(false);
		UpdateVisuals();
	}

	public override void OnPress()
	{
		lightSwitchAudio.pitch = isSwitchedOn ? 1f : 1.1f;
		lightSwitchAudio.PlayOneShot(lightSwitchAudio.clip);

		// the light switch can be on, but the light itself
		// might not necessarily be on if we run out of power
		isSwitchedOn = !isSwitchedOn;

		if(!Generator.HasPower)
		{
			HelpManager.Instance.ShowText("THERES NO POWER");
		}

		if(isSwitchedOn)
		{
			if(Generator.HasPower)
			{
				SetEmitting(true);
			}
		}
		else
		{
			SetEmitting(false);
		}

		UpdateVisuals();
	}

	void SetEmitting(bool emitting)
	{
		if(IsEmitting != emitting)
		{
			if(emitting)
			{
				EmitCount++;
			}
			else
			{
				EmitCount--;
			}
		}

		IsEmitting = emitting;

		if(IsEmitting)
		{
			OnStartedEmitting?.Invoke();
		}
	}

	void UpdateVisuals()
	{
		lightContainer.SetActive(IsEmitting);
		rend.material = isSwitchedOn ? onMaterial : offMaterial;
	}

	void OnGeneratorRanOutOfFuel()
	{
		SetEmitting(false);
		UpdateVisuals();
	}

	void OnGeneratorRefueled()
	{
		if(isSwitchedOn)
		{
			SetEmitting(true);
			UpdateVisuals();
		}
	}
}
