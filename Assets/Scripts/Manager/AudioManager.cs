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


    //public bool shouldLoop = false;
    public static AudioManager Instance;
    //public AudioClip backGroundMusic;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }
    private void Start()
    {
        backGroudSource = backGroudSource.GetComponent<AudioSource>();
        shotsSource = shotsSource.GetComponent<AudioSource>();
    }

    public void SoundsEventTrigger(SoundEvents soundEvents, bool shouldLoop)
    {
        switch (soundEvents)
        {
            case SoundEvents.Pistal:
                PistalClips(shouldLoop, shotsSource);
                break;
            case SoundEvents.Rifle:
                RifleClips(shouldLoop, shotsSource);
                break;
            case SoundEvents.Railgun:
                RailGunClips(shouldLoop, shotsSource);
                break;
            case SoundEvents.ChainGun:
                ChainGunClips(shouldLoop, shotsSource);
                break;
            case SoundEvents.BackGroundMusic:
                BackGroundMusic(shouldLoop, backGroudSource);
                break;
            case SoundEvents.Ambient:
                AmbientClip(shouldLoop, backGroudSource);
                break;

        }
    }
    public void SoundsEventTrigger(SoundEvents soundEvents, bool shouldLoop, AudioSource PlaySource)
    {
        switch (soundEvents)
        {
            case SoundEvents.Pistal:
                PistalClips(shouldLoop, PlaySource);
                break;
            case SoundEvents.Rifle:
                RifleClips(shouldLoop, PlaySource);
                break;
            case SoundEvents.Railgun:
                RailGunClips(shouldLoop, PlaySource);
                break;
            case SoundEvents.ChainGun:
                ChainGunClips(shouldLoop, PlaySource);
                break;
            case SoundEvents.BackGroundMusic:
                BackGroundMusic(shouldLoop, PlaySource);
                break;
            case SoundEvents.Ambient:
                AmbientClip(shouldLoop, PlaySource);
                break;

        }
    }

    public void PistalClips(bool loop, AudioSource PlaySource)
    {
        int randomInt = Random.Range(0, pistalClips.Count-1);
        PlaySource.clip = pistalClips[randomInt].audio;
        if (loop)
        {
            PlaySource.loop = true;
        }
        else
        {
            PlaySource.loop = false;
        }
        PlaySource.Play();
    }
    private void RifleClips(bool loop, AudioSource PlaySource)
    {
        int randomInt = Random.Range(0, rifleClips.Count - 1);
        PlaySource.clip = rifleClips[randomInt].audio;
        if (loop)
        {
            PlaySource.loop = true;
        }
        else
        {
            PlaySource.loop = false;
        }
        PlaySource.Play();
    }
    private void RailGunClips(bool loop, AudioSource PlaySource)
    {
        int randomInt = Random.Range(0, railGunClips.Count - 1);
        PlaySource.clip = railGunClips[randomInt].audio;
        if (loop)
        {
            PlaySource.loop = true;
        }
        else
        {
            PlaySource.loop = false;
        }
        PlaySource.Play();
    }
    private void ChainGunClips(bool loop, AudioSource PlaySource)
    {
        int randomInt = Random.Range(0, chainGunClips.Count - 1);
        PlaySource.clip = chainGunClips[randomInt].audio;
        if (loop)
        {
            PlaySource.loop = true;
        }
        else
        {
            PlaySource.loop = false;
        }
        PlaySource.Play();
    }
    private void BackGroundMusic(bool loop, AudioSource PlaySource)
    {
            int randomInt = Random.Range(0, backGroundClips.Count);
            PlaySource.clip = backGroundClips[randomInt].audio;
        if (loop)
        {
            PlaySource.loop = true;
        }
        else
        {
            PlaySource.loop = false;
        }
        PlaySource.Play();
    }
    private void AmbientClip(bool loop, AudioSource PlaySource)
    {
        PlaySource.clip = ambientClips.audio;
        if (loop)
        {
            PlaySource.loop = true;
        }
        else
        {
            PlaySource.loop = false;
        }
        PlaySource.Play();
    }
}


