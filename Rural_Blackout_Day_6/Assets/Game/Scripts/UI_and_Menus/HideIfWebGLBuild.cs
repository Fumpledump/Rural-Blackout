using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIfWebGLBuild : MonoBehaviour
{
#if UNITY_WEBGL
	void Start()
	{
		gameObject.SetActive(false);
	}
#endif
}
