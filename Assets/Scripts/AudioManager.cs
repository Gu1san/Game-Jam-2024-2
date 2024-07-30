using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider mainAudio;
    [SerializeField] Slider musicAudio;
    [SerializeField] Slider fxAudio;

    private void Start()
    {
        LoadVolume();
    }

    public void ChangeMainVolume()
    {
        float volume = mainAudio.value;
        audioMixer.SetFloat("Main", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("MainVolume", volume);
    }

    public void ChangeMusicVolume()
    {
        float volume = musicAudio.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void ChangeSFXVolume()
    {
        float volume = fxAudio.value;
        audioMixer.SetFloat("FX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        mainAudio.value = PlayerPrefs.GetFloat("MainVolume");
        musicAudio.value = PlayerPrefs.GetFloat("MusicVolume");
        fxAudio.value = PlayerPrefs.GetFloat("SFXVolume");
        ChangeMainVolume();
        ChangeMusicVolume();
        ChangeSFXVolume();
    }
}
