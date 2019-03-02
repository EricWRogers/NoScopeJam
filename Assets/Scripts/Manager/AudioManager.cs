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
                PistalClips(shouldLoop);
                break;
            case SoundEvents.Rifle:
                RifleClips(shouldLoop);
                break;
            case SoundEvents.Railgun:
                RailGunClips(shouldLoop);
                break;
            case SoundEvents.ChainGun:
                ChainGunClips(shouldLoop);
                break;
            case SoundEvents.BackGroundMusic:
                BackGroundMusic(shouldLoop);
                break;
            case SoundEvents.Ambient:
                AmbientClip(shouldLoop);
                break;

        }
    }

    public void PistalClips(bool loop)
    {
        int randomInt = Random.Range(0, pistalClips.Count-1);
        shotsSource.clip = pistalClips[randomInt].audio;
        if (loop)
        {
            shotsSource.loop = true;
        }
        else
        {
            shotsSource.loop = false;
        }
        shotsSource.Play();
    }

    private void RifleClips(bool loop)
    {
        int randomInt = Random.Range(0, rifleClips.Count - 1);
        shotsSource.clip = rifleClips[randomInt].audio;
        if (loop)
        {
            shotsSource.loop = true;
        }
        else
        {
            shotsSource.loop = false;
        }
        shotsSource.Play();
    }
    private void RailGunClips(bool loop)
    {
        int randomInt = Random.Range(0, railGunClips.Count - 1);
        shotsSource.clip = railGunClips[randomInt].audio;
        if (loop)
        {
            shotsSource.loop = true;
        }
        else
        {
            shotsSource.loop = false;
        }
        shotsSource.Play();
    }
    private void ChainGunClips(bool loop)
    {
        int randomInt = Random.Range(0, chainGunClips.Count - 1);
        shotsSource.clip = chainGunClips[randomInt].audio;
        if (loop)
        {
            shotsSource.loop = true;
        }
        else
        {
            shotsSource.loop = false;
        }
        shotsSource.Play();
    }

    private void BackGroundMusic(bool loop)
    {
            int randomInt = Random.Range(0, backGroundClips.Count);
            backGroudSource.clip = backGroundClips[randomInt].audio;
        if (loop)
        {
            backGroudSource.loop = true;
        }
        else
        {
            backGroudSource.loop = false;
        }
        backGroudSource.Play();
    }

    private void AmbientClip(bool loop)
    {
        backGroudSource.clip = ambientClips.audio;
        if (loop)
        {
            backGroudSource.loop = true;
        }
        else
        {
            backGroudSource.loop = false;
        }
        backGroudSource.Play();
    }
}


