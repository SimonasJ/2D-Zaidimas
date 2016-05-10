using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

    public Slider slider;
    void Awake()
    {
        GetComponent<AudioSource>().volume = Player.GetFlost("CurVol");
        slider.value = GetComponent<AudioSource>().volume;
    }

    public void ChangeScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void LoadScene(string loadName)
    {
        Application.LoadLevel(loadName);
    }

    public void VolumeControl(float volumeControl)
    {
        GetComponent<AudioSource>().volume = volumeControl;
        PlayerPrefs.SetFloat("CurVol", GetComponent<AudioSource>().volume);
    }
}
