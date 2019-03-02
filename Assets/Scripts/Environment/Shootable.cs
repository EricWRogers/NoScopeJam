using System;
using UnityEngine;
using UnityEngine.Events;

public class Shootable : MonoBehaviour
{
    [System.Serializable]
    public class DamageEvent : UnityEvent<float>
    {

    }

    public DamageEvent OnShot;

    public void Shoot(float _damage)
    {
        OnShot.Invoke(_damage);
    }
}