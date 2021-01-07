using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    static public Audio instance;
    public AudioClip clickClip;
    public AudioClip SlideCardsClip;

    private AudioSource audioSrc;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        audioSrc = GetComponent<AudioSource>();
    }

    public void PlayClickAudio()
    {
        audioSrc.clip = clickClip;
        audioSrc.Play();
    }

    public void PlaySlideCardsAudio()
    {
        audioSrc.clip = SlideCardsClip;
        audioSrc.Play();
    }

    
}
