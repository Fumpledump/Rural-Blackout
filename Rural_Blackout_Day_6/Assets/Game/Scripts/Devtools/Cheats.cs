using UnityEngine;

public class Cheats : MonoBehaviour {

    [Header("K - Change Time")]

    [SerializeField]
    private Clock _clock;
    [SerializeField]
    private float _rawTimeScaleOverride = 4f;

    [Header("L - Toggle Light Switches")]

    [SerializeField]
    private LightSwitch[] _lightSwitches;

    void Update() {

        if(Input.GetKeyDown(KeyCode.O)) {

            Debug.Log("O - All cheats activated");
            ChangeClockRawTimeScale();
            ToggleLightSwitches();

        }

        if(Input.GetKeyDown(KeyCode.K)) {
            
            Debug.Log("K - Change Time: Up to " + (60f / _rawTimeScaleOverride) + " seconds match 1 ingame hour)");
            ChangeClockRawTimeScale();
        }

        if(Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("L - Toggle Light Switches");
            ToggleLightSwitches();
        }

    }

    private void ToggleLightSwitches() {

        for(int i = 0; i < _lightSwitches.Length; i++) {
            _lightSwitches[i].OnPress();
        }

    }

    private void ChangeClockRawTimeScale() {

        _clock.rawTimeScale = _rawTimeScaleOverride;

    }

}
