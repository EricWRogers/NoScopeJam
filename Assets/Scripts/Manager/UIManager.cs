using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject optionsMenu;
    public GameObject soundsMenu;
    public GameObject controlsMenu;

    public Slider masterVolume;
    public Slider backgroundVolume;
    public Slider fxVolume;

    public AudioManager audioManager;

    private bool fromStartMenu = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        startMenu.SetActive(false);
    }
    public void Continue
    ()
    {
        startMenu.SetActive(false);
    }
    public void Options()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
        fromStartMenu = true;
    }
    public void Settings()
    {
        optionsMenu.SetActive(true);
        fromStartMenu = false;
    }

    public void MasterVolume()
    {
        audioManager.backGroudSource.volume = masterVolume.value;
        audioManager.shotsSource.volume = masterVolume.value;
    }
    public void BackGroundMusic()
    {
        audioManager.backGroudSource.volume = backgroundVolume.value;
    }
    public void FXVolume()
    {
        audioManager.shotsSource.volume = fxVolume.value;
    }

    public void SoundMenu()
    {
        soundsMenu.SetActive(true);
    }
    public void ControlsMenu()
    {
        controlsMenu.SetActive(true);
    }
}
