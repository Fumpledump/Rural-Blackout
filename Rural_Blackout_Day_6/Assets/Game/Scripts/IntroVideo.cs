using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideo : MonoBehaviour
{
    public GameObject playerFlashlight;
    public bool IntroPlaying;
    public List<LightSwitch> lights;

    private VideoPlayer videoPlayer;

    public bool playIntro;
    public bool playOutro;

    public static IntroVideo Instance;

    [SerializeField] AudioSource shutdownAudio;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Get reference to the VideoPlayer component
        videoPlayer = GetComponent<VideoPlayer>();

        // Subscribe to the loopPointReached event
        videoPlayer.loopPointReached += OnVideoFinished;


        if (!playIntro)
        {
            return;
        }

        IntroPlaying = true;

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
        if (playOutro)
        {
            SceneManager.LoadScene("Menu");
        }else if (playIntro)
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

            HelpManager.Instance.BlackoutNotification();

            this.gameObject.SetActive(false); // Hide Screen
        }
    }
}
