using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeze : MonoBehaviour
{
	[SerializeField] FirstPersonController controller;
	[SerializeField] swim swimController;
	[SerializeField] PlayerInteraction interaction;
	[SerializeField] PlayerWalkNoise walkNoise;
	[SerializeField] GameObject interactCanvas;
	[SerializeField] GameObject sprintCanvas;
	[SerializeField] GameObject playerCamera;
	
	[SerializeField] bool isSwim;
	public static PlayerFreeze Instance { get; private set; }

	void Awake()
	{
		Instance = this;
	}

	public void Freeze()
	{
		if (isSwim)
		{
			swimController.enabled = false;
		}
		else
		{
		    controller.enabled = false;
		}
		
		interaction.enabled = false;
		walkNoise.enabled = false;
		interactCanvas.SetActive(false);
		sprintCanvas.SetActive(false);
		playerCamera.SetActive(false);
	}

	public void Unfreeze()
	{
		if (isSwim)
		{
			swimController.enabled = true;
		}
		else
		{
		    controller.enabled = true;
		}

		interaction.enabled = true;
		walkNoise.enabled = true;
		interactCanvas.SetActive(true);
		sprintCanvas.SetActive(true);
		playerCamera.SetActive(true);
	}
}
