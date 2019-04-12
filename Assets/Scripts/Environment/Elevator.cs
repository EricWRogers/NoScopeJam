using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float rideDuration = 5f;

    private bool openBackRequested = false;
    private bool frontDoorClosed = false;
    private bool inRide = false;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
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
        if (!frontDoorClosed)
        {
            Debug.Log("frontDoorClosed: " + frontDoorClosed);
            return;
        }
        
        if (!this.inRide)
        {
            Debug.Log("this.inRide: " + this.inRide);
            _animator.SetBool("Back Open", true);
            RaisingWater.Instance.InitRaisingWater();
        }
        else
        {
            Debug.Log("this.inRide: " + this.inRide);
            openBackRequested = true;
        }
    }

    public void closeDoorsInFront()
    {
        if (!frontDoorClosed && !inRide)
        {
            frontDoorClosed = true;
            _animator.SetBool("Front Close", true);
            
            StartCoroutine(WaitAndEndRide(rideDuration));
        }
    }

    IEnumerator WaitAndEndRide(float delay)
    {
        RaisingWater.Instance.InitDrainWater();

        inRide = true;
        yield return new WaitForSeconds(1);
        _animator.SetBool("Go Up", true);
        yield return new WaitForSeconds(delay);
        inRide = false;

        if (true)
        {
            openDoorBehind();
        }
    }
}