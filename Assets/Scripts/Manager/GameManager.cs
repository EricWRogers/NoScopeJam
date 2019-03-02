using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] LevelsGOS;
    public GameObject[] CheckPointsGOS;
    public GameObject PlayerPrefabGO;
    private GameObject PlayerCurrentGO;
    private GameObject CurrentLevelGO;
    public int StartLevelNumerator;
    private int CurrentLevelNumerator;
    public bool ShowAllLevels = false;
    public bool WorkingOnLevel = false;
    public static GameManager Instance;
    private void Awake()
    {
        if(Instance == null) {
            Instance = this;
        }else{
            Destroy(this.gameObject);
            return;
        }
        // Set Current Level
        CurrentLevelNumerator = StartLevelNumerator;
        // Load PlayerPref

        // SpawnLevels
        if ( !WorkingOnLevel ) {
            if ( ShowAllLevels ) {
                for ( int i = LevelsGOS.Length - 1; i > -1; i-- ) {
                    LoadLevel( i );
                }
            } else {
                LoadLevel( StartLevelNumerator );
            }
        }
        // SpawnPlayer
        PlayerCurrentGO = Instantiate( PlayerPrefabGO, CheckPointsGOS[ StartLevelNumerator ].transform );
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    private void LoadLevel( int slot )
    {
        CurrentLevelNumerator = slot;
        Instantiate(LevelsGOS[ slot ]);
    }
    private void DestoryLevel( float LifeTime, GameObject KillMe )
    {
        Destroy(KillMe, LifeTime);
    }
    public void SavePlayerStats(string slot)
    {
        // GameObject TempPlayer = GameObject.FindGameObjectWithTag("Player");
        // string json = JsonUtility.ToJson(myObject);
        // Check if string of slots is in there
        if( PlayerPrefs.HasKey("SlotsNames")) {
            Slots TempSlots = JsonUtility.FromJson<Slots>(PlayerPrefs.GetString("SlotsNames"));
            TempSlots.slots.Add(slot);
            string json = JsonUtility.ToJson(TempSlots);
            PlayerPrefs.SetString("SlotsName", json);
        }
        //PlayerPrefs.SetString(slot, m_PlayerName);
    }
    public void LoadPlayerStats(string slot)
    {
        string json = PlayerPrefs.GetString(slot);
        // myObject = JsonUtility.FromJson<MyClass>(json);
    }
    public List<string> PlayerStatsKeys()
    {
        if( PlayerPrefs.HasKey("SlotsNames")) {
            Slots TempSlots = JsonUtility.FromJson<Slots>(PlayerPrefs.GetString("SlotsNames"));
            return TempSlots.slots;
        }
        return new List<string>{"null"};
    }

}
[System.Serializable]
public class Slots
{
    public List<string> slots;
}