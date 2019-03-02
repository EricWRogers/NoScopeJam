using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private List<GameObject> TurretGOS;
    private void Awake()
    {
        int NumTurrets = Random.Range(1, TurretGOS.Count - 1);
        for ( int i = NumTurrets; i > -1; i-- ) {
            int temp = Random.Range(0, TurretGOS.Count - 1);
            TurretGOS[temp].SetActive(false);
            TurretGOS.RemoveAt(temp);
        }
    }
}
