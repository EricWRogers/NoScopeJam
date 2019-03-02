using System;
using UnityEngine;
using UnityEngine.Events;

public class Shootable : MonoBehaviour
{
    public UnityEvent OnShot;

    public void Shoot()
    {
        OnShot.Invoke();
    }
}