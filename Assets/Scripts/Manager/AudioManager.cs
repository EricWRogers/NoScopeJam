using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEvents
{
    Pistal,
    Rifle,
    Railgun,
    ChainGun,
    BackGroundMusic,
    Ambient
}
[System.Serializable]
public class Clips
{
    public AudioClip audio;
    public bool shouldLoop = false;
}

public class AudioManager : MonoBehaviour
{
    public List<Clips> pistalClips = new List<Clips>();
    public List<Clips> rifleClips = new List<Clips>();
    public List<Clips> railGunClips = new List<Clips>();
    public List<Clips> chainGunClips = new List<Clips>();
    public List<Clips> backGroundClips = new List<Clips>();

    public Clips ambientClips;

    public AudioSource backGroudSource;
    public AudioSource shotsSource;

    //public AudioClip backGroundMusic;
    private void Start()
    {
        backGroudSource = backGroudSource.GetComponent<AudioSource>();
        shotsSource = shotsSource.GetComponent<AudioSource>();
    }

    public void SoundsEventTrigger(SoundEvents soundEvents)
    {
        switch (soundEvents)
        {
            case SoundEvents.Pistal:
                PistalClips();
                break;
            case SoundEvents.Rifle:
                RifleClips();
                break;
            case SoundEvents.Railgun:
                RailGunClips();
                break;
            case SoundEvents.ChainGun:
                ChainGunClips();
                break;
            case SoundEvents.BackGroundMusic:
                break;

        }
    }

    public void PistalClips()
    {
        int randomInt = Random.Range(0, pistalClips.Count-1);
        shotsSource.clip = pistalClips[randomInt].audio;
        if (pistalClips[randomInt].shouldLoop)
        {
            shotsSource.loop = true;
        }
        else
        {
            shotsSource.loop = false;
        }
        shotsSource.Play();
    }

    private void RifleClips()
    {
        int randomInt = Random.Range(0, rifleClips.Count - 1);
        shotsSource.clip = rifleClips[randomInt].audio;
        if (rifleClips[randomInt].shouldLoop)
        {
            shotsSource.loop = true;
        }
        else
        {
            shotsSource.loop = false;
        }
        shotsSource.Play();
    }
    private void RailGunClips()
    {
        int randomInt = Random.Range(0, railGunClips.Count - 1);
        shotsSource.clip = railGunClips[randomInt].audio;
        if (railGunClips[randomInt].shouldLoop)
        {
            shotsSource.loop = true;
        }
        else
        {
            shotsSource.loop = false;
        }
        shotsSource.Play();
    }
    private void ChainGunClips()
    {
        int randomInt = Random.Range(0, chainGunClips.Count - 1);
        shotsSource.clip = chainGunClips[randomInt].audio;
        if (chainGunClips[randomInt].shouldLoop)
        {
            shotsSource.loop = true;
        }
        else
        {
            shotsSource.loop = false;
        }
        shotsSource.Play();
    }

    private void BackGroundMusic()
    {
            int randomInt = Random.Range(0, backGroundClips.Count);
            backGroudSource.clip = backGroundClips[randomInt].audio;
        backGroudSource.Play();
    }

    private void AmbientClip()
    {
        backGroudSource.clip = ambientClips.audio;
        backGroudSource.Play();
    }
}


