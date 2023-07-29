using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class URLHandler
{
	[DllImport("__Internal")]
	private static extern void OpenTab(string url);

	public static void OpenURL(string url)
	{
#if !UNITY_EDITOR && UNITY_WEBGL
		OpenTab(url);
#else
		Application.OpenURL(url);
#endif
	}
}
