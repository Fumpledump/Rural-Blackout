using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] CanvasGroup cg;
	[SerializeField] FirstPersonController player;

	bool paused;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
		{
			paused = !paused;

			UpdateActive();
		}
	}

	public void Resume()
	{
		paused = false;

		UpdateActive();
	}

	public void Menu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("Menu");
	}

	public void GiveIn()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("WinRuins");
	}

	void UpdateActive()
	{
		if(paused)
		{
			cg.alpha = 1f;
			cg.interactable = true;
			cg.blocksRaycasts = true;
			Time.timeScale = 0f;
			player.cameraCanMove = false;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			cg.alpha = 0f;
			cg.interactable = false;
			cg.blocksRaycasts = false;
			Time.timeScale = 1f;
			player.cameraCanMove = true;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
}
