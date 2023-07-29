using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotProductTest : MonoBehaviour
{
	[SerializeField] Transform target;

	void Update()
	{
		Debug.Log(Vector3.Dot(transform.forward, (target.position - transform.position).normalized));
		Debug.DrawRay(transform.position, (target.position - transform.position), Color.red);
	}
}
