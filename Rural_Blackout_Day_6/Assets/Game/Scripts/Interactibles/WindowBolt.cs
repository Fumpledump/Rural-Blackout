using UnityEngine;

public class WindowBolt : Interactable {
    public GameObject[] disable;
    public GameObject enable;

    [SerializeField]
    private PlayerInteraction _playerInteraction;

    [SerializeField]
    private AudioSource _boltWindowsAudio;

    public override void OnPress() {

        if(_playerInteraction._planksCollectedCount > 0) {
            Bolt();
            _playerInteraction._planksCollectedCount--;
        }

    }
    public override void OnRelease() {

    }

    public void Bolt() {

        _boltWindowsAudio.PlayOneShot(_boltWindowsAudio.clip);

        for (int i = 0; i < disable.Length; i++) {
            if (disable[i].GetComponent<BreakInPoint>()!=null) {
                disable[i].GetComponent<BreakInPoint>().StopBreakIn();
            }
            disable[i].SetActive(false);
        }
        enable.SetActive(true);

        Destroy(transform.GetComponent<WindowBolt>());
    }

}