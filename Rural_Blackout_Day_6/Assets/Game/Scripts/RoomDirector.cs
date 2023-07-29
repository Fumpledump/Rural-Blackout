using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoomDirector : MonoBehaviour
{
	Room[] rooms;

	[HideInInspector]
	public List<Room> _roomsWithoutLights = null;

	public float timeSinceStart, maxActivityTime, startDelay, startInterval, endInterval, startTime, endTime, currentTime;
	private float currentDelay;
	private bool hasStarted;
	public bool gameOver;

	public static RoomDirector Instance { get; private set; }
	
	public AudioClip audioClip;

	bool enemyInside;

	void Awake()
	{
		enemyInside = false;
		Instance = this;
		rooms = GetComponentsInChildren<Room>();
		Generator.OnRefueled += OnGeneratorRefueledForFirstTime;
		BreakInPoint.OnSuccessfulBreakIn += OnSuccessfulBreakIn;
	}

	void OnDestroy()
	{
		Generator.OnRefueled -= OnGeneratorRefueledForFirstTime;
		BreakInPoint.OnSuccessfulBreakIn -= OnSuccessfulBreakIn;
	}

	void OnSuccessfulBreakIn()
	{
		enemyInside = true;
		StopAllCoroutines();
	}

	void OnGeneratorRefueledForFirstTime()
	{
		HelpManager.Instance.ShowText("I FEEL A SENSE OF DREAD");
		Generator.OnRefueled -= OnGeneratorRefueledForFirstTime;
		StartCoroutine(StartBreakIn());
	}

	void Update()
    {
        if (hasStarted)
        {
			timeSinceStart += Time.deltaTime;

			UpdateDelay();

			//generator fail
			List<Room> roomsWithoutLights = new List<Room>();

			foreach (var room in rooms)
			{
				if (!room.LightSwitchOn)
				{
					roomsWithoutLights.Add(room);
				}
			}

			if (roomsWithoutLights.Count <= 5)
			{
				Debug.Log("you are using way too many LIGHTS!, WHO DO YOU THINK IS GOING TO PAY THE EECTRICITY BILL, IM TURNING THEM OFF!(and generator goes to 0 as a punisment)");
        		HelpManager.Instance.ShowText("You used too many lights...");
				//generator looses all of its fuel
				Generator.FuelPercent = 0;
				//turn the lights off
    
			    foreach (var room in rooms)
			    {
			    	if (room.LightSwitchOn)
			    	{
			    		room.lightSwitch.isSwitchedOn = false;
						
			    	}
			    }
			}
		}
	}

	IEnumerator StartBreakIn()
    {
		yield return new WaitForSeconds(startDelay);
		hasStarted = true;

		// needs to be called here or else currentDelay is 0 and we create
		// 2 enemies outside
		UpdateDelay();

		while(true)
        {
			if (gameOver) yield return null;
			if (enemyInside) yield return null;
			Debug.Log(enemyInside);

			_roomsWithoutLights = new List<Room>();

			foreach(var room in rooms) {
				if(room.breakInPoints == null || room.lightSwitch == null) {
					// Rooms without a breakInPoint are NOT added
				} else if(!room.LightSwitchOn) {
					_roomsWithoutLights.Add(room);
				}
			}

			if(_roomsWithoutLights.Count == 0 || _roomsWithoutLights == null) {
				Debug.Log("No rooms have lights off, so we can't start a break in!");
				yield return null;
			} else {
				int random = Random.Range(0, _roomsWithoutLights.Count);
				_roomsWithoutLights[random].StartBreakIn();
				Debug.Log("roomsWithoutLights is " + _roomsWithoutLights[random].name);
				yield return new WaitForSeconds(currentDelay);
			}
		}
	}

	void UpdateDelay()
	{
		currentDelay = Mathf.Lerp(startInterval, endInterval, timeSinceStart / maxActivityTime);
		currentTime = Mathf.Lerp(startTime, endTime, timeSinceStart / maxActivityTime);
	}
}
