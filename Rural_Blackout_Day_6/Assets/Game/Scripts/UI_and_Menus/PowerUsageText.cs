using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUsageText : MonoBehaviour
{
	[SerializeField] TMP_Text text;

	void Update()
	{
		// its a jam game ok plz spare me
		text.text = $"PWR USAGE: {LightSwitch.EmitCount + 1}X";
	}
}
