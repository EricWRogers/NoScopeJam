using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> TurretGOS;

    private void Awake()
    {
        int NumTurrets = Random.Range(0, TurretGOS.Count - 1);
        Debug.Log("NumTurrets: " + NumTurrets);
        /*for (int i = NumTurrets; i > -1; i--)
        {
            int temp = Random.Range(0, TurretGOS.Count - 1);
            Debug.Log("Destory Turrets: " + temp);
            Debug.Log("TurretFalse");
            TurretGOS[temp].SetActive(false);
            TurretGOS.RemoveAt(temp);
        }*/
    }
}