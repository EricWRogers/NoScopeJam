using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] LevelsGOS;
    public GameObject[] CheckPointsGOS;
    public GunType[] GunTypes;
    public GameObject PlayerPrefabGO;
    public GameObject PlayerCurrentGO;
    private GameObject CurrentLevelGO;
    public int StartLevelNumerator;
    private int CurrentLevelNumerator;
    public bool ShowAllLevels = false;
    public bool WorkingOnLevel = false;
    public bool StartFromMain = false;
    public static GameManager Instance;

    private const string defaultSlotName = "default";

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

        // Set Current Level
        CurrentLevelNumerator = StartLevelNumerator;
        // Load PlayerPref

        if (!StartFromMain)
        {
            NewGame();
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void NewGame()
    {
        // SpawnLevels
        if (!WorkingOnLevel)
        {
            if (ShowAllLevels)
            {
                for (int i = LevelsGOS.Length - 1; i > -1; i--)
                {
                    LoadLevel(i);
                }
            }
            else
            {
                LoadLevel(StartLevelNumerator);
            }
        }

        if (PlayerPrefabGO)
        {
            // SpawnPlayer
            PlayerCurrentGO = Instantiate(PlayerPrefabGO, CheckPointsGOS[StartLevelNumerator].transform.position,
                Quaternion.identity);

            // Deactivate Camera
            if (StartFromMain)
            {
                GameObject Camera = GameObject.FindGameObjectWithTag("MainMenuCamera");
                Camera.SetActive(false);
            }
        }
        
        RaisingWater.Instance.InitRaisingWater();
    }

    private void LoadLevel(int slot)
    {
        CurrentLevelNumerator = slot;
        Instantiate(LevelsGOS[slot]);
        AudioManager.Instance.SoundsEventTrigger(SoundEvents.BackGroundMusic, true);
    }

    private void DestoryLevel(float LifeTime, GameObject KillMe)
    {
        Destroy(KillMe, LifeTime);
    }

    public void SavePlayerStats(string slot)
    {
        // GameObject TempPlayer = GameObject.FindGameObjectWithTag("Player");
        string json = PlayerStats.Instance.GetJsonString();

        PlayerPrefs.SetString(slot, json);
        // Check if string of slots is in there
        if (PlayerPrefs.HasKey("SlotsNames"))
        {
            Slots TempSlots = JsonUtility.FromJson<Slots>(PlayerPrefs.GetString("SlotsNames"));
            TempSlots.slots.Add(slot);
            json = JsonUtility.ToJson(TempSlots);
            PlayerPrefs.SetString("SlotsNames", json);
        }
        else
        {
            Slots NSlots = new Slots();
            NSlots.slots.Add(slot);
            json = JsonUtility.ToJson(NSlots);
            PlayerPrefs.SetString("SlotsNames", json);
        }
    }

    public void LoadPlayerStats(string slot)
    {
        if (PlayerPrefs.HasKey(slot))
        {
            string json = PlayerPrefs.GetString(slot);
            PlayerStats.Instance.LoadFromJsonString(json);
        }
    }

    public void SaveDefaultPlayerStats()
    {
        SavePlayerStats(defaultSlotName);
    }

    public void LoadDefaultPlayerStats()
    {
        LoadPlayerStats(defaultSlotName);
    }

    public bool HasASlot()
    {
        return PlayerStatsKeys().Count > 0;
    }

    public List<string> PlayerStatsKeys()
    {
        if (PlayerPrefs.HasKey("SlotsNames"))
        {
            Slots TempSlots = JsonUtility.FromJson<Slots>(PlayerPrefs.GetString("SlotsNames"));
            return TempSlots.slots;
        }

        return new List<string>();
    }
}

[System.Serializable]
public class Slots
{
    public List<string> slots;
}