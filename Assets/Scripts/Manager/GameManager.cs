using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] LevelsGOS;
    public GameObject[] CheckPointsGOS;
    public GameObject PlayerGO;
    public GameObject CurrentLevelGO;
    public string SaveLevel;
    public string SavePlayer;
    public int StartLevelNumerator;
    private int CurrentLevelNumerator;
    public bool ShowAllLevels = false;
    public bool WorkingOnLevel = false;
    private void Awake()
    {
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
        Instantiate( PlayerGO, CheckPointsGOS[ StartLevelNumerator ].transform );
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
    


}
