using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSwapper : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void ChangeToMusic(AudioClip newMusic)
    {
        audioSource.Stop();
        audioSource.clip = newMusic;
        audioSource.Play();
    }
}
