using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    //https://www.youtube.com/watch?v=DU7cgVsU2rM - guide for SFX Manager

    public static SFXManager instance;

    [SerializeField] private AudioSource SFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn gameObject
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        //assign audioClip to gameObject
        audioSource.clip = audioClip;

        //assign volume of audioClip
        audioSource.volume = volume;

        //play audioClip
        audioSource.Play();

        //get length of audioClip
        float clipLength = audioSource.clip.length;

        //destroy gameObject when finished
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayLoopSFXClip(Transform parentPos, AudioClip audioClip, Transform audioPos, float volume)
    {
        //spawn gameObject
        AudioSource audioSource = Instantiate(SFXObject, audioPos.position, Quaternion.identity);

        //set gameObject position to parent object

        //assign audioClip to gameObject

        //assign volume of audioClip

        //play audioClip while x is true, destroy when x is false

    }

}
