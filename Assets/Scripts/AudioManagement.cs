using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManagement : MonoBehaviour
{
    public Toggle audioToggle;
    public Toggle vibrateToggle;
    private static bool isNotVibrated = false;

    void Start()
    {
        isNotVibrated = PlayerPrefs.GetInt("NotVibrate", 0) == 0;
        if (vibrateToggle != null) {
            vibrateToggle.isOn = isNotVibrated;
            vibrateToggle.onValueChanged.AddListener(SetVibration);
        }
    }

    public void SetVibration(bool isOn) {
        isNotVibrated = isOn;
        PlayerPrefs.SetInt("NotVibrate", isOn ? 0 : 1);
        PlayerPrefs.Save();
    }

    public void MuteHander(bool mute) {
        if (mute) {
            AudioListener.volume = 0;
        } else {
            AudioListener.volume = 1;
        }
    }

    public static void Vibrate() {
        if (isNotVibrated) {
        } else {
            Handheld.Vibrate();
        }
    }
}
