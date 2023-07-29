using UnityEngine;

public class AmbientColorChanger : MonoBehaviour {

    [Header("References")]

    [SerializeField]
    private Clock _clock;

    [Header("Lighting - AmbientColor")]

    public bool _setAmbientLight = false;

    public Color _ambientColorFrom = Color.black;
    public Color _ambientColorTo = Color.gray;
    public Color _ambientColorCurrent = Color.black;
    public bool _overrideInitialAmbientColor = true;

    [Header("Lighting - AmbientColor Lerp")]

    public int _startClockTime = 5; // matches timeAtRing value
    private int _startClockTimeIndex = 4; // matches timesAtRings array index
    public float _durationClockTime = 1; // duration of the color lerp in ingame clock time
    public bool _autoCalculateDuration = true;
    private float _timeElapsed = 0; // time passed since the color lerp started

    public enum LerpType { Linear, Exponential }
    public LerpType _lerpType = LerpType.Exponential;
    [Tooltip("Set a value between 1f and 2f.")]
    public float _lerpExponent = 2f;

    [Header("Only Check When Testing")]

    [SerializeField]
    private bool _devLerpPreview = false;

    private void Start() {
        if(_overrideInitialAmbientColor) {
            _ambientColorCurrent = _ambientColorFrom;
            RenderSettings.ambientLight = _ambientColorCurrent;
        }

        if(_autoCalculateDuration) {
            // Calculates the duration from the given _startClockTime to the winTime
            _durationClockTime = _clock.winTime - _startClockTime;
        }

        if( _clock.timesAtRings == null || _clock.timesAtRings.Length <= 0) {
            Debug.Log("The timesAtRings array must not be null or empty.");
            return;
        }

        for(int i = 0; i < _clock.timesAtRings.Length; i++) {
            if(_clock.timesAtRings[i].timeAtRing == _startClockTime) {
                _startClockTimeIndex = i;
                return;
            } else if(i == _clock.timesAtRings.Length - 1) {
                Debug.Log("The timesAtRings array does not contain the given _startClockTime as value for timeAtRing. " +
                    "Check the TimesAtRings array on Clock GameObject with the same named script component.");
                return;
            }
        }
    }

    private void Update() {

        if(_clock.timesAtRings[_startClockTimeIndex].hasRung) {
            _setAmbientLight = true;
        } else if(_timeElapsed >= _durationClockTime) {
            _setAmbientLight = false;
        }

        if(_setAmbientLight || _devLerpPreview) {
            _ambientColorCurrent = Color.Lerp(_ambientColorFrom, _ambientColorTo, GetLerpTime());
            RenderSettings.ambientLight = _ambientColorCurrent;
        }

    }

    private float GetLerpTime() {

        if(_devLerpPreview) {
            return Mathf.PingPong(Time.time, 1);
        }

        _timeElapsed += Time.deltaTime;
        float lerpTime = Mathf.Clamp01(
            _timeElapsed / _clock.ConvertFromClockTimeToSeconds(_durationClockTime));

        switch(_lerpType) {
            case LerpType.Linear:
                break;
            case LerpType.Exponential:
                lerpTime = Mathf.Pow(lerpTime, _lerpExponent);
                break;
            default:
                break;
        }

        return lerpTime;

    }
}