using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinChime : MonoBehaviour
{
	[SerializeField] AudioSource source;

	void Start()
	{
		StartCoroutine(PlayChimes());
	}

	IEnumerator PlayChimes()
	{
		for(int i = 0; i < 5; i++)
		{
			yield return new WaitForSeconds(1.5f);
			source.Play();
		}

		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene("Menu");
	}
}
