using UnityEngine;

public class WatchScript : MonoBehaviour {

    public Animator hand;
    private bool watchOpen;

    [SerializeField]
    private AudioSource _clothRubbingAudio;

    private float _audioPitch = 1f;

    private void Start() {
        _audioPitch = _clothRubbingAudio.pitch;
    }

    private void Update() {

        if (Input.GetKeyDown("tab")) {
            if (watchOpen==false) {
                watchOpen = true;
                hand.SetBool("Watch", true);

                _clothRubbingAudio.pitch = _audioPitch;
                _clothRubbingAudio.PlayOneShot(_clothRubbingAudio.clip);
            } else {
                watchOpen = false;
                hand.SetBool("Watch", false);

                if(_audioPitch - 0.1f > 0f) {
                    _clothRubbingAudio.pitch = _audioPitch - 0.1f;
                }
                _clothRubbingAudio.PlayOneShot(_clothRubbingAudio.clip);
            }
        }

    }
}