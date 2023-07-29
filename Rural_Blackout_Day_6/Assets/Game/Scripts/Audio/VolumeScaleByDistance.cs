using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeScaleByDistance : MonoBehaviour
{
	[SerializeField] Transform target;
	[SerializeField] float maxDistance;
	[SerializeField] AudioSource source;

	void Update()
	{
		source.volume = Mathf.Lerp(1f, 0f, Vector3.Distance(transform.position, target.position) / maxDistance);
	}
}
