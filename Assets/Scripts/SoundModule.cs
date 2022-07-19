using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundModule : MonoBehaviour
{
    public AudioClip bite;
    public AudioClip blubb;
    public AudioClip dead;

    public AudioClip backgroundLoop;

    private AudioSource fxSource;
    //private AudioSource backgroundSource;

    public static SoundModule Instance;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        fxSource = GetComponent<AudioSource>();

    }

    public void PlayBlubb()
    {
        fxSource.PlayOneShot(blubb);
    }
    public void PlayBite()
    {
        fxSource.PlayOneShot(bite);
    }
    public void PlayDead()
    {
        fxSource.PlayOneShot(dead);
    }
}
