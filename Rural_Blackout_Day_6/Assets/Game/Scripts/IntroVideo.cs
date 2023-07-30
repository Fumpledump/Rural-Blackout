using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class IntroVideo : MonoBehaviour
{
    public GameObject playerFlashlight;
    public bool IntroPlaying;
    public List<LightSwitch> lights;

    private VideoPlayer videoPlayer;

    public bool playIntro;

    public static IntroVideo Instance;

    [SerializeField] AudioSource shutdownAudio;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (!playIntro)
        {
            return;
        }

        IntroPlaying = true;

        // Get reference to the VideoPlayer component
        videoPlayer = GetComponent<VideoPlayer>();

        // Subscribe to the loopPointReached event
        videoPlayer.loopPointReached += OnVideoFinished;

        // Turn off flashlight
        playerFlashlight.SetActive(false);

        // Turn on Lights
        for (int i = 0; i < lights.Count; i++)
        {
            Debug.Log("Light " + i + "turned on");
            lights[i].SetEmitting(true);
            lights[i].UpdateVisualsIntro();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // Video playback has finished
        Debug.Log("Intro Video finished!");
        IntroPlaying = false;

        // Start Blackout
        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].SetEmitting(false);
            lights[i].UpdateVisualsIntroEnd();
        }

        shutdownAudio.Play();

        // Turn on flashlight
        playerFlashlight.SetActive(true);

        this.gameObject.SetActive(false); // Hide Screen
    }
}
