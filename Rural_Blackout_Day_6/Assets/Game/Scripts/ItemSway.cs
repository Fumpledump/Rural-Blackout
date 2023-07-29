using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSway : MonoBehaviour
{
	[SerializeField] float moveAmount;
	[SerializeField] float rotateAmount;
	[SerializeField] float maxMove;
	[SerializeField] float maxRotate;
	[Range(0, 0.999f)]
	[SerializeField] float moveSmoothAmount;
	[Range(0, 0.999f)]
	[SerializeField] float rotateSmoothAmount;

	Vector3 initialPosition;
	Quaternion initialRotation;

	float weight = 1f;

	void Start()
	{
		initialPosition = transform.localPosition;
		initialRotation = transform.localRotation;
	}

	void Update()
	{
		float mouseX = Input.GetAxisRaw("Mouse X") * weight;
		float mouseY = Input.GetAxisRaw("Mouse Y") * weight;

		float moveX = mouseX * moveAmount;
		float moveY = mouseY * moveAmount;
		moveX = Mathf.Clamp(moveX, -maxMove, maxMove);
		Vector3 finalPos = new Vector3(moveX, 0, moveY);
		transform.localPosition = SmoothLerp(transform.localPosition, finalPos + initialPosition, moveSmoothAmount);

		float rotateX = mouseY * rotateAmount;
		float rotateY = mouseX * rotateAmount;
		rotateX = Mathf.Clamp(rotateX, -maxRotate, maxRotate);
		rotateY = Mathf.Clamp(rotateY, -maxRotate, maxRotate);
		Quaternion finalRot = Quaternion.Euler(new Vector3(-rotateX, rotateY, rotateY));
		transform.localRotation = SmoothLerp(transform.localRotation, finalRot * initialRotation, rotateSmoothAmount);
	}

	// this is necessary to have the same smoothing at different framerates. it works pretty well
	// https://stackoverflow.com/questions/43720669/lerp-with-time-deltatime
	Vector3 SmoothLerp(Vector3 a, Vector3 b, float speed)
	{
		return Vector3.Lerp(a, b, 1 - Mathf.Pow(1 - speed, Time.deltaTime * 60));
	}

	Quaternion SmoothLerp(Quaternion a, Quaternion b, float speed)
	{
		return Quaternion.Lerp(a, b, 1 - Mathf.Pow(1 - speed, Time.deltaTime * 60));
	}
}