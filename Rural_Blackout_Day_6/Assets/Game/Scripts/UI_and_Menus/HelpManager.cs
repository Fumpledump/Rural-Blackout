using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpManager : MonoBehaviour
{
	public static HelpManager Instance { get; private set; }

	public GameObject playerFlashlight;

	[SerializeField] TMP_Text helpText;

	Coroutine coroutine;

	void Awake()
	{
		Instance = this;
	}

	public void ShowText(string text)
	{
		if(coroutine != null)
		{
			StopCoroutine(coroutine);
		}

		StartCoroutine(I_ShowText(text));
	}

	IEnumerator I_ShowText(string text)
	{
		helpText.gameObject.SetActive(true);
		helpText.text = text;

		yield return new WaitForSeconds(3f);

		helpText.gameObject.SetActive(false);
	}

	public void BlackoutNotification()
    {
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}

		StartCoroutine(BlackoutNotificationHelper());
	}

	IEnumerator BlackoutNotificationHelper()
	{
		yield return new WaitForSeconds(2f);

		// Turn on flashlight
		playerFlashlight.SetActive(true);

		StartCoroutine(I_ShowText("I should turn on my generator..."));
	}
}
