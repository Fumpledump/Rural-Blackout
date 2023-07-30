using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinChime : MonoBehaviour
{
	[SerializeField] AudioSource source;
	[SerializeField] float timeBeforeMenu;
	[SerializeField] bool playChimes;

	void Start()
	{
		StartCoroutine(PlayChimes());
	}

	IEnumerator PlayChimes()
	{
        if (playChimes)
        {
			for (int i = 0; i < 5; i++)
			{
				yield return new WaitForSeconds(1.5f);
				source.Play();
			}
		}

		yield return new WaitForSeconds(timeBeforeMenu);
		SceneManager.LoadScene("Menu");
	}
}
