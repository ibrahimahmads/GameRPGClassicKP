using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadVolume();
        }else{
            SetMusicVolume();
            SetSFXVolume();
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume()
    {
        float volumeMusic = musicSlider.value;
        myMixer.SetFloat("music",Mathf.Log10(volumeMusic)*20);
        PlayerPrefs.SetFloat("musicVolume", volumeMusic);
        // GameManager.instance.volmsc= volumeMusic; 
    }

    public void SetSFXVolume()  
    {
        float volumeSfx = sfxSlider.value;
        myMixer.SetFloat("sfx",Mathf.Log10(volumeSfx)*20);
        PlayerPrefs.SetFloat("sfxVolume", volumeSfx);
        // GameManager.instance.volsfx= volumeSfx;
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetSFXVolume();
    }
}
