using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public struct TimesAtRingData {
    public int timeAtRing;
    public bool hasRung;
}

public class Clock : MonoBehaviour {

    [Header("References")]

    public Generator gen;
    public GameObject enemy;
    public TMP_Text clockText;
    [SerializeField] string winScreen;

    [Header("Time Settings")]

    public float timeSinceStart = 0f;
    private float rawTimeSinceStart = 0f;
    [Tooltip("The rawTimeScale changes how the timeSinceStart references the rawTimeSinceStart " +
        "allowing to change the player's real time matching an ingame hour. " +
        "A rawTimeScale of 1 matches the original timing. " +
        "A rawTimeScale of 2 means time differences are referenced as passing double as fast. " +
        "A rawTimeScale of 0.5 means time differences are referenced as passing half as fast.")]
    public float rawTimeScale = 2f;
    public TimesAtRingData[] timesAtRings;
    public float winTime = 6f;

    [Header("States")]

    public bool hasStarted = false;
    public bool hasFinished = false;

    private void Start() {
        Generator.OnRefueled += FuelFirstTime;
    }

    private void Update() {
        if(hasFinished) {
            return;
        }

        if(hasStarted) {
            //rawTimeSinceStart += Time.deltaTime;
            rawTimeSinceStart = rawTimeSinceStart + Time.deltaTime * rawTimeScale;
            timeSinceStart = Mathf.Round(rawTimeSinceStart / 60f);
        }

        TimeChecks();
    }

    private void TimeChecks() {
        for(int i = 0; i < timesAtRings.Length; i++) {
            if(timeSinceStart >= winTime) {
                EndGame();
                return;
            }

            if(timesAtRings[i].timeAtRing == timeSinceStart && !timesAtRings[i].hasRung) {
                timesAtRings[i].hasRung = true;

                clockText.text = timeSinceStart + ":00 am";
                if(GameObject.FindGameObjectWithTag("WatchText") != null) {
                    GameObject.FindGameObjectWithTag("WatchText").GetComponent<TMP_Text>().text = timeSinceStart + ":00";
                }

                StartCoroutine(PlaySounds((int) timeSinceStart));
            }
        }
    }

    private void FuelFirstTime() {
        Generator.OnRefueled -= FuelFirstTime;
        hasStarted = true;
    }

    private void EndGame() {
        if(RoomDirector.Instance.gameOver == true) {
            return;
        }
        Debug.Log("Won Game");
        enemy.SetActive(false);
        RoomDirector.Instance.gameOver = true;
		SceneManager.LoadScene(winScreen);
    }

    private IEnumerator PlaySounds(int hour) {
        int i = 0;
        while(i < hour) {
            GetComponent<AudioSource>().Play();
            i++;
            yield return new WaitForSeconds(1.5f);
        }
    }

    /* 
     * Utility methods for converting between
     * - unpaused real play time (after FuelFirstTime() happened) and
     * - ingame "clock" time
     */

    public float ConvertFromClockTimeToSeconds(float clockTime) {
        return clockTime * 60 / rawTimeScale;
    }

    public float ConvertFromSecondsToClockTime(float timeInSeconds) {
        return timeInSeconds / 60 * rawTimeScale;
    }
}