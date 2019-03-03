using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject creditMenu;
    public GameObject optionsMenu;
    public GameObject soundsMenu;
    public GameObject controlsMenu;
    public GameObject startMenuButton;
    public GameObject hud;
    public GameObject hudCanvas;

    public Slider masterVolume;
    public Slider backgroundVolume;
    public Slider fxVolume;

    public GameObject helpingObject;
    public Animator helpingTextAnim;
    public Text helpfulText;
    private bool animPlaying;
    private float helpingTimer = 0f;

    public TextMeshProUGUI bigGameOverText;
    public TextMeshProUGUI littleGameOverText;
    public GameObject gameOverMenu;

    private float lerpTime =0f;

    public Image gunType;
    public Text healthText;
    public Text ammoText;
    public Image boostMeter;

    private bool pauseEnable = false;
    private bool outOfMenus = false;

    public AudioManager audioManager;

    private bool fromStartMenu = false;

    public static UIManager Instance;
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
        Time.timeScale = 0;
    }
    public bool OutofMenu
    {
        get { return outOfMenus; }
    }
    void Start()
    {
        helpingTextAnim = helpingObject.GetComponent<Animator>();
        hudCanvas.SetActive(false);
        
        PlayerStats.Instance.gameOverEvent.AddListener(GameOver);
    }

    // Update is called once per frame
    void Update()
    {
        if (animPlaying)
        {
            if (helpingTimer > 0f)
            {
                helpingTimer -= Time.deltaTime;
                if (helpingTimer < 0f)
                {
                    helpingTextAnim.SetTrigger("IN");
                    helpingTextAnim.ResetTrigger("OUT");
                    helpingTimer = 0f;
                    animPlaying = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) ||Input.GetKeyDown(KeyCode.JoystickButton7) && !pauseEnable && outOfMenus)
        {
            Settings();

        }
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7) && pauseEnable && !outOfMenus)
        {
            OptionsBack();
        }
    }

    private void FixedUpdate()
    {
        gunType.sprite = PlayerStats.Instance.CurrentGun.image;
        healthText.text = "" + PlayerStats.Instance.Health.ToString("F0");
        ammoText.text = "" + PlayerStats.Instance.CurrentAmmo.ToString("F0");
        boostMeter.fillAmount = Mathf.Lerp(boostMeter.fillAmount, PlayerStats.Instance.ThrusterCharge, lerpTime);
        lerpTime += Time.deltaTime;
        if (lerpTime > 1.0f)
        {
            lerpTime = 0f;
        }

    }

    public void PlayGame()
    {
        hudCanvas.SetActive(true);
        startMenu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        outOfMenus = true;
    }
    public void Credits()
    {
        startMenu.SetActive(false);
        creditMenu.SetActive(true);
    }
    public void CreditsBack()
    {
        startMenu.SetActive(true);
        creditMenu.SetActive(false);
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
        pauseEnable = true;
        outOfMenus = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void OptionsBack()
    {
        if (pauseEnable)
        {
            Time.timeScale = 1;
            outOfMenus = true;
            pauseEnable = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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

    public void HelpText(string message)
    {
        animPlaying = true;
        helpfulText.text = message;
        helpingTextAnim.SetTrigger("OUT");
        helpingTextAnim.ResetTrigger("IN");
        helpingTimer = 10f;
    }
    public void StartMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver(bool isWin)
    {
        outOfMenus = false;
        if (isWin)
        {
            bigGameOverText.text = "Congradulations!";
            littleGameOverText.text = "You won!";
            gameOverMenu.SetActive(true);
        }
        else
        {
            bigGameOverText.text = "Game Over!";
            littleGameOverText.text = "Try again.";
            gameOverMenu.SetActive(true);
        }
    }

}
