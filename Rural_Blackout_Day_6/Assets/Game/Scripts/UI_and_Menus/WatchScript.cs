using UnityEngine;

public class WatchScript : MonoBehaviour 
{
    public Animator hand;
    [SerializeField] private GameObject arm;
    [SerializeField] private bool armShown;
    [SerializeField] private bool watchOpen;

    [SerializeField]
    private AudioSource _clothRubbingAudio;

    private float _audioPitch = 1f;

    private void Start() {
        _audioPitch = _clothRubbingAudio.pitch;
    }

    private void Update() 
    {

        if (Input.GetKeyDown("tab")) 
        {
            if (watchOpen==false) {
                watchOpen = true;
                hand.SetBool("Watch", true);

                arm.SetActive(true);
                armShown = true;

                _clothRubbingAudio.pitch = _audioPitch;
                _clothRubbingAudio.PlayOneShot(_clothRubbingAudio.clip);
            } else {
                watchOpen = false;
                hand.SetBool("Watch", false);

                if (_audioPitch - 0.1f > 0f) {
                    _clothRubbingAudio.pitch = _audioPitch - 0.1f;
                }
                _clothRubbingAudio.PlayOneShot(_clothRubbingAudio.clip);
            }

            
        }

        if (!watchOpen && armShown)
        {
            // Loop through all the layers of the Animator
            for (int i = 0; i < hand.layerCount; i++)
            {
                // Check if the current layer has any animation state playing
                if (hand.GetCurrentAnimatorStateInfo(i).normalizedTime < 1f)
                {
                    return;
                }
            }

            arm.SetActive(false);
            armShown = false;
        }

    }
}