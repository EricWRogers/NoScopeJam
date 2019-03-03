﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject startMenu;
    public GameObject optionsMenu;
    public GameObject saveQuitButtton;

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
        saveQuitButtton.SetActive(false);
    }
    public void Settings()
    {
        optionsMenu.SetActive(true);
        fromStartMenu = false;
        saveQuitButtton.SetActive(true);
    }
}
