using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> TurretGOS;
    public GameObject Water;
    public float WaterRaiseSpeed;

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

    private void Update()
    {
        RaiseWater();
    }

    private void RaiseWater()
    {
        if (Water)
        {
            Vector3 targetPosition = Water.transform.position;
            targetPosition.y += WaterRaiseSpeed;

            Water.transform.position = Vector3.Lerp(Water.transform.position, targetPosition, Time.deltaTime);
        }
    }
}