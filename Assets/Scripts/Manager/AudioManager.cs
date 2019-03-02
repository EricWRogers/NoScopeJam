using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEvents
{
    StartingGame,
    NPCTalking,
    NPCTalkingToEmily,
    PickingUpItem,
    HidesInBathroom,
    PathBlocked,
    AnxietyLax,
    NPCStandingBetween,
    FireAlarm,
    Success,
    PassesOut,
    ArgumentBreaksOut,
    CardDeclined,
    HighAnxiety,
    CheckingOut,
    SeeingEx,
    ShelfFalls,
    PhoneMusic,
    BackGroundMusic,
}
[System.Serializable]
public class Clips
{
    public int playerVoice;
    public AudioClip audio;
    public bool hasPlayed = false;
}

public class AudioManager : MonoBehaviour
{
    public List<Clips> startingGameClips = new List<Clips>();
    public List<Clips> npcTalkingToEmilyClips = new List<Clips>();
    public List<Clips> pickingUpItemClips = new List<Clips>();
    public List<Clips> hidesInBathroomClips = new List<Clips>();
    public List<Clips> pathBlockedClips = new List<Clips>();
    public List<Clips> anxietyLaxClips = new List<Clips>();
    public List<Clips> npcStandingBetweenClips = new List<Clips>();
    public List<Clips> fireAlarmClips = new List<Clips>();
    public List<Clips> successClips = new List<Clips>();
    public List<Clips> passesOutClips = new List<Clips>();
    public List<Clips> argumentBreaksOutClips = new List<Clips>();
    public List<Clips> cardDeclinedClips = new List<Clips>();
    public List<Clips> highAnxietyClips = new List<Clips>();
    public List<Clips> checkingOutClips = new List<Clips>();
    public List<Clips> seeingExClips = new List<Clips>();
    public List<Clips> shelfFallsClips = new List<Clips>();
    public List<Clips> phoneMusicClips = new List<Clips>();
    public List<Clips> backGroundClips = new List<Clips>();

    public AudioSource backGroudSource;
    public AudioSource voicesSource;

    public int currentVoice = 0;

    //public AudioClip backGroundMusic;
    private void Start()
    {
        backGroudSource = backGroudSource.GetComponent<AudioSource>();
        voicesSource = voicesSource.GetComponent<AudioSource>();
    }

    public void SoundsEventTrigger(SoundEvents soundEvents)
    {
        switch (soundEvents)
        {
            case SoundEvents.StartingGame:
                StartingGame();
                break;
            case SoundEvents.NPCTalking:
                break;
            case SoundEvents.NPCTalkingToEmily:
                NPCTalkingToEmily();
                break;
            case SoundEvents.PickingUpItem:
                PickingUpItem();
                break;
            case SoundEvents.HidesInBathroom:
                HidesInBathroom();
                break;
            case SoundEvents.PathBlocked:
                PathBlocked();
                break;
            case SoundEvents.AnxietyLax:
                AnxietyLax();
                break;
            case SoundEvents.NPCStandingBetween:
                NPCStandingBetween();
                break;
            case SoundEvents.FireAlarm:
                FireAlarm();
                break;
            case SoundEvents.Success:
                Success();
                break;
            case SoundEvents.PassesOut:
                PassesOut();
                break;
            case SoundEvents.ArgumentBreaksOut:
                ArgumentBreaksOut();
                break;
            case SoundEvents.CardDeclined:
                CardDeclined();
                break;
            case SoundEvents.HighAnxiety:
                HighAnxiety();
                break;
            case SoundEvents.CheckingOut:
                CheckingOut();
                break;
            case SoundEvents.SeeingEx:
                SeeingEx();
                break;
            case SoundEvents.ShelfFalls:
                ShelfFalls();
                break;
            case SoundEvents.PhoneMusic:
                PhoneMusic();
                break;
            case SoundEvents.BackGroundMusic:
                break;

        }
    }

    public void StartingGame()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, startingGameClips.Count);
            if (!startingGameClips[randomInt].hasPlayed)
            {
                currentVoice = startingGameClips[randomInt].playerVoice;
                voicesSource.clip = startingGameClips[randomInt].audio;
                voicesSource.Play();
                startingGameClips[randomInt].hasPlayed = true;
            }
        }
    }

    private void NPCTalkingToEmily()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, npcTalkingToEmilyClips.Count);
            if (!npcTalkingToEmilyClips[randomInt].hasPlayed)
            {
                currentVoice = npcTalkingToEmilyClips[randomInt].playerVoice;
                voicesSource.clip = npcTalkingToEmilyClips[randomInt].audio;
                voicesSource.Play();
                npcTalkingToEmilyClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void PickingUpItem()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, pickingUpItemClips.Count);
            if (!pickingUpItemClips[randomInt].hasPlayed)
            {
                currentVoice = pickingUpItemClips[randomInt].playerVoice;
                voicesSource.clip = pickingUpItemClips[randomInt].audio;
                voicesSource.Play();
                pickingUpItemClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void HidesInBathroom()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, hidesInBathroomClips.Count);
            if (!hidesInBathroomClips[randomInt].hasPlayed)
            {
                currentVoice = hidesInBathroomClips[randomInt].playerVoice;
                voicesSource.clip = hidesInBathroomClips[randomInt].audio;
                voicesSource.Play();
                hidesInBathroomClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void PathBlocked()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, pathBlockedClips.Count);
            if (!pathBlockedClips[randomInt].hasPlayed)
            {
                currentVoice = pathBlockedClips[randomInt].playerVoice;
                voicesSource.clip = pathBlockedClips[randomInt].audio;
                voicesSource.Play();
                pathBlockedClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void AnxietyLax()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, anxietyLaxClips.Count);
            if (!anxietyLaxClips[randomInt].hasPlayed)
            {
                currentVoice = anxietyLaxClips[randomInt].playerVoice;
                voicesSource.clip = anxietyLaxClips[randomInt].audio;
                voicesSource.Play();
                anxietyLaxClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void NPCStandingBetween()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, npcStandingBetweenClips.Count);
            if (!npcStandingBetweenClips[randomInt].hasPlayed)
            {
                currentVoice = npcStandingBetweenClips[randomInt].playerVoice;
                voicesSource.clip = npcStandingBetweenClips[randomInt].audio;
                voicesSource.Play();
                npcStandingBetweenClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void FireAlarm()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, fireAlarmClips.Count);
            if (!fireAlarmClips[randomInt].hasPlayed)
            {
                currentVoice = fireAlarmClips[randomInt].playerVoice;
                voicesSource.clip = fireAlarmClips[randomInt].audio;
                voicesSource.Play();
                fireAlarmClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void Success()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, successClips.Count);
            if (!successClips[randomInt].hasPlayed)
            {
                currentVoice = successClips[randomInt].playerVoice;
                voicesSource.clip = successClips[randomInt].audio;
                voicesSource.Play();
                successClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void PassesOut()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, passesOutClips.Count);
            if (!passesOutClips[randomInt].hasPlayed)
            {
                currentVoice = passesOutClips[randomInt].playerVoice;
                voicesSource.clip = passesOutClips[randomInt].audio;
                voicesSource.Play();
                passesOutClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void ArgumentBreaksOut()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, argumentBreaksOutClips.Count);
            if (!argumentBreaksOutClips[randomInt].hasPlayed)
            {
                currentVoice = argumentBreaksOutClips[randomInt].playerVoice;
                voicesSource.clip = argumentBreaksOutClips[randomInt].audio;
                voicesSource.Play();
                argumentBreaksOutClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void CardDeclined()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, cardDeclinedClips.Count);
            if (!cardDeclinedClips[randomInt].hasPlayed)
            {
                currentVoice = cardDeclinedClips[randomInt].playerVoice;
                voicesSource.clip = cardDeclinedClips[randomInt].audio;
                voicesSource.Play();
                cardDeclinedClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void HighAnxiety()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, highAnxietyClips.Count);
            if (!highAnxietyClips[randomInt].hasPlayed)
            {
                currentVoice = highAnxietyClips[randomInt].playerVoice;
                voicesSource.clip = highAnxietyClips[randomInt].audio;
                voicesSource.Play();
                highAnxietyClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void CheckingOut()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, checkingOutClips.Count);
            if (!checkingOutClips[randomInt].hasPlayed)
            {
                currentVoice = checkingOutClips[randomInt].playerVoice;
                voicesSource.clip = checkingOutClips[randomInt].audio;
                voicesSource.Play();
                checkingOutClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void SeeingEx()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, seeingExClips.Count);
            if (!seeingExClips[randomInt].hasPlayed)
            {
                currentVoice = seeingExClips[randomInt].playerVoice;
                voicesSource.clip = seeingExClips[randomInt].audio;
                voicesSource.Play();
                seeingExClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void ShelfFalls()
    {
        if (!voicesSource.isPlaying)
        {
            int randomInt = Random.Range(0, shelfFallsClips.Count);
            if (!shelfFallsClips[randomInt].hasPlayed)
            {
                currentVoice = shelfFallsClips[randomInt].playerVoice;
                voicesSource.clip = shelfFallsClips[randomInt].audio;
                voicesSource.Play();
                shelfFallsClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void PhoneMusic()
    {
        if (!backGroudSource.isPlaying)
        {
            int randomInt = Random.Range(0, phoneMusicClips.Count);
            if (!phoneMusicClips[randomInt].hasPlayed)
            {
                currentVoice = phoneMusicClips[randomInt].playerVoice;
                backGroudSource.clip = phoneMusicClips[randomInt].audio;
                backGroudSource.Play();
                phoneMusicClips[randomInt].hasPlayed = true;
            }
        }
    }
    private void BackGroundMusic()
    {
        if (!backGroudSource.isPlaying)
        {
            int randomInt = Random.Range(0, backGroundClips.Count);
            if (!backGroundClips[randomInt].hasPlayed)
            {
                currentVoice = backGroundClips[randomInt].playerVoice;
                backGroudSource.clip = backGroundClips[randomInt].audio;
                backGroudSource.Play();
                backGroundClips[randomInt].hasPlayed = true;
            }
        }
    }
}


