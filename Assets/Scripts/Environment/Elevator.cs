﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Door frontDoors;
    public Door backDoors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
        }
    }

    private void openDoorInFront()
    {
        frontDoors.OpenDoors();
    }

    private void closeDoorsBehind()
    {
        backDoors.CloseDoors();
    }
}
