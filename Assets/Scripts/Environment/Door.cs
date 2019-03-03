using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    public float doorSpeed;

    // Start is called before the first frame update
    void Start()
    {
        OpenDoors(true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenDoors(bool instant = false)
    {
        gameObject.SetActive(false);
    }

    public void CloseDoors(bool instant = false)
    {
        gameObject.SetActive(true);
    }
}