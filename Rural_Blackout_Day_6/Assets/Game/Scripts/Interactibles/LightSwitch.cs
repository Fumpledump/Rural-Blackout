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
		if (!IntroVideo.Instance.IntroPlaying)
		{
			SetEmitting(false);
			UpdateVisuals();
		}
	}

	public override void OnPress()
	{
		lightSwitchAudio.pitch = isSwitchedOn ? 1f : 1.1f;
		lightSwitchAudio.PlayOneShot(lightSwitchAudio.clip);

		// the light switch can be on, but the light itself
		// might not necessarily be on if we run out of power
		isSwitchedOn = !isSwitchedOn;

		if (!Generator.HasPower)
		{
			HelpManager.Instance.ShowText("THERES NO POWER");
		}

		if (isSwitchedOn)
		{
			if (Generator.HasPower)
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

	public void SetEmitting(bool emitting)
	{
		if (IsEmitting != emitting)
		{
			if (emitting)
			{
				EmitCount++;
			}
			else
			{
				EmitCount--;
			}
		}

		IsEmitting = emitting;

		if (IsEmitting)
		{
			OnStartedEmitting?.Invoke();
		}
	}

	public void UpdateVisuals()
	{
		lightContainer.SetActive(IsEmitting);
		rend.material = isSwitchedOn ? onMaterial : offMaterial;
	}

	public void UpdateVisualsIntro()
	{
		rend.material = onMaterial;
		lightContainer.SetActive(true);
	}

	public void UpdateVisualsIntroEnd()
	{
		rend.material = offMaterial;
		lightContainer.SetActive(false);
	}

	void OnGeneratorRanOutOfFuel()
	{
		SetEmitting(false);
		UpdateVisuals();
	}

	void OnGeneratorRefueled()
	{
		if (isSwitchedOn)
		{
			SetEmitting(true);
			UpdateVisuals();
		}
	}
}
