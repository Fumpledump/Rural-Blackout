using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SecuritySystem : MonoBehaviour
{
	[SerializeField] GameObject normalVolume;
	[SerializeField] GameObject cameraVolume;
	[SerializeField] SecurityCamera[] cameras;
	[SerializeField] TMP_Text activeCameraNameText;
	[SerializeField] GameObject cameraUI;
	[SerializeField] GameObject noPowerUI;
	[SerializeField] AudioSource cameraStatic;
	[SerializeField] AudioSource cameraSwitch;

	int activeIndex = 0;

	bool systemOnline = false;

	void Awake()
	{
		Generator.OnRanOutOfFuel += OnGeneratorRanOutOfFuel;
		Generator.OnRefueled += OnGeneratorRefueled;
	}

	void OnDestroy()
	{
		Generator.OnRanOutOfFuel -= OnGeneratorRanOutOfFuel;
		Generator.OnRefueled -= OnGeneratorRefueled;
	}

	void OnGeneratorRanOutOfFuel()
	{
		noPowerUI.SetActive(true);
	}

	void OnGeneratorRefueled()
	{
		noPowerUI.SetActive(false);
	}

	public void Activate()
	{
		if(!Generator.HasPower)
			OnGeneratorRanOutOfFuel();

		normalVolume.SetActive(false);
		cameraVolume.SetActive(true);
		cameraUI.SetActive(true);

		UpdateActiveCamera();

		systemOnline = true;

		PlayerFreeze.Instance.Freeze();

		cameraStatic.Play();
		cameraSwitch.PlayOneShot(cameraSwitch.clip);
	}

	public void Deactivate()
	{
		normalVolume.SetActive(true);
		cameraVolume.SetActive(false);
		cameraUI.SetActive(false);

		for(int i = 0; i < cameras.Length; i++)
		{
			cameras[i].gameObject.SetActive(false);
		}

		systemOnline = false;

		PlayerFreeze.Instance.Unfreeze();

		cameraStatic.Stop();
	}

	void Update()
	{
		if(systemOnline && Generator.HasPower)
		{
			if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
			{
				cameraSwitch.PlayOneShot(cameraSwitch.clip);

				activeIndex--;

				if(activeIndex < 0)
				{
					activeIndex = cameras.Length - 1;
				}

				UpdateActiveCamera();
			}
			else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
			{
				cameraSwitch.PlayOneShot(cameraSwitch.clip);

				activeIndex++;

				if(activeIndex >= cameras.Length)
				{
					activeIndex = 0;
				}

				UpdateActiveCamera();
			}
		}
	}

	void UpdateActiveCamera()
	{
		for(int i = 0; i < cameras.Length; i++)
		{
			cameras[i].gameObject.SetActive(i == activeIndex);

			if(i == activeIndex)
			{
				activeCameraNameText.text = $"{(i + 1)}/{cameras.Length} {cameras[i].gameObject.name}";
			}
		}
	}
}
