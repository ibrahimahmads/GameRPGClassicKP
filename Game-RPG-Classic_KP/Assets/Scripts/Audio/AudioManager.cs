using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] public AudioSource sfxSource;
    [Header("----- AudioClip -----")]
    [Header("----- AudioClips -----")]
    public AudioClip background;
    public AudioClip[] sfxClips;
    

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxClips.Length)
        {
            sfxSource.PlayOneShot(sfxClips[index]);
        }
        else
        {
            Debug.LogWarning("Index SFX tidak valid!");
        }
    }

    public void PlayLoop(int index)
    {
        sfxSource.clip = sfxClips[index]; // Tetapkan AudioClip ke AudioSource
        sfxSource.loop = true;           // Aktifkan mode loop
        sfxSource.Play(); 
    }

    public void StopSFX()
    {
        sfxSource.loop = false;
        sfxSource.Stop();
    }
}
