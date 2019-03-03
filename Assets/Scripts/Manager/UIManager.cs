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
    public GameObject startMenuButton;
    public GameObject hud;

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
        hud.SetActive(true);
    }
    public void Continue()
    {
        startMenu.SetActive(false);
        hud.SetActive(true);
    }
    public void Options()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
        startMenuButton.SetActive(false);
        fromStartMenu = true;
    }
    public void Settings()
    {
        optionsMenu.SetActive(true);
        fromStartMenu = false;
        startMenuButton.SetActive(true);
    }

    public void OptionsBack()
    {
        optionsMenu.SetActive(false);
        soundsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        if (fromStartMenu)
        {
            startMenu.SetActive(true);
        }
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
        controlsMenu.SetActive(false);
    }
    public void ControlsMenu()
    {
        controlsMenu.SetActive(true);
        soundsMenu.SetActive(false);
    }
}
