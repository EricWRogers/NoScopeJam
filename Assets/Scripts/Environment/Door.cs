using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;
    public float openDistance;

    public float doorSpeed;

    public enum DoorState
    {
        None,
        Opening,
        Closing,
    }

    public DoorState doorState = DoorState.None;

    private Vector3 leftOrig;
    private Vector3 rightOrig;
    private Vector3 leftEnd;
    private Vector3 rightEnd;

    // Start is called before the first frame update
    void Start()
    {
        leftOrig = leftDoor.localPosition;
        rightOrig = rightDoor.localPosition;

        leftEnd = leftOrig;
        leftEnd.z += openDistance;

        rightEnd = rightOrig;
        rightEnd.z -= openDistance;

        OpenDoors(true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (doorState)
        {
            case DoorState.Opening:
                OpenDoors();
                break;
            case DoorState.Closing:
                CloseDoors();
                break;
        }
    }

    void OpenDoors(bool instant = false)
    {
        if (instant)
        {
            leftDoor.localPosition = leftEnd;
            rightDoor.localPosition = rightEnd;
        }
        else
        {
            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftEnd, Time.deltaTime * doorSpeed);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightEnd, Time.deltaTime * doorSpeed);
        }
    }

    void CloseDoors(bool instant = false)
    {
        if (instant)
        {
            leftDoor.localPosition = leftOrig;
            rightDoor.localPosition = rightOrig;
        }
        else
        {
            leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftOrig, Time.deltaTime * doorSpeed);
            rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightOrig, Time.deltaTime * doorSpeed);
        }
    }
}