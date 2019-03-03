using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Door frontDoor;
    public Door backDoor;
    public float rideDuration = 5f;

    private bool openBackRequested = false;
    private bool frontDoorClosed = false;
    private bool inRide = false;

    // Start is called before the first frame update
    void Start()
    {
        frontDoor.OpenDoors(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            closeDoorsInFront();
        }
    }

    public void openDoorBehind()
    {
        if (!this.inRide)
        {
            backDoor.OpenDoors();
        }
        else
        {
            openBackRequested = true;
        }
    }

    public void closeDoorsInFront()
    {
        if (!frontDoorClosed && !inRide)
        {
            frontDoorClosed = true;
            frontDoor.CloseDoors();
            
            StartCoroutine(WaitAndEndRide(rideDuration));
        }
    }

    IEnumerator WaitAndEndRide(float delay)
    {
        RaisingWater.Instance.InitDrainWater();

        inRide = true;
        yield return new WaitForSeconds(delay);
        inRide = false;

        if (openBackRequested)
        {
            openDoorBehind();
        }
    }
}