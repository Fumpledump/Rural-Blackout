using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakInPoint : MonoBehaviour
{
	[SerializeField] Renderer rend;
	[SerializeField] Material[] brokenMaterials;
	[SerializeField] AudioSource brokenSound;
	[SerializeField] GameObject outsideEnemyPrefab;
	[SerializeField] GameObject insidePoint;
	[SerializeField] AudioClip scareAwayClip;

	public bool windowIsSupposedToBeHighUp;
	
	public static event Action OnSuccessfulBreakIn;

	GameObject instantiatedEnemy;

	bool breakingIn;
	float breakInProgress;
	public float breakInTime = 40;

	void BreakIn()
	{
		rend.materials = brokenMaterials;

		brokenSound.Play();
	}

	public void StartBreakIn()
	{
		Vector3 spawnpoint = transform.position;
	if(!windowIsSupposedToBeHighUp)
	{//who ever wrote this raycst code ....I hate you, its not your fault I just had a bad time trying to figure out why the angler fish was spawning so low, turns out this code makes it spawn close to ground, but in the underwater level the windows are high up, so in in some windows I made it not run
		if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f))
		{
			spawnpoint = hit.point;
		}
	}
		instantiatedEnemy = Instantiate(outsideEnemyPrefab, spawnpoint, transform.rotation);
		breakingIn = true;
		breakInTime = RoomDirector.Instance.currentTime;
		breakInProgress = 0f;
	}

	public void StopBreakIn()
	{
		if(breakingIn)
		{
			breakingIn = false;
			AudioSource.PlayClipAtPoint(scareAwayClip, transform.position);
			Destroy(instantiatedEnemy);
		}
	}

	void SpawnIndoorEnemy()
	{
		var insideEnemy = GameObject.FindObjectOfType<EnemyAI>(true);

		// this should never be called but just in case
		if(!insideEnemy.gameObject.activeSelf)
		{
			insideEnemy.transform.position = insidePoint.transform.position;
			print("inside point is " + insidePoint.name);
			insideEnemy.gameObject.SetActive(true);
			OnSuccessfulBreakIn?.Invoke();
		}
	}

	void Update()
	{
		if(breakingIn)
		{
			breakInProgress += Time.deltaTime;

			if(breakInProgress >= breakInTime)
			{
				BreakIn();
				StopBreakIn();
				SpawnIndoorEnemy();
			}
		}
	}
}
