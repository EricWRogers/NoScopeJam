using System;
using UnityEngine;
using UnityEngine.Events;

public class Shootable : MonoBehaviour
{
    [System.Serializable]
    public class DamageEvent : UnityEvent<float, Vector3>
    {

    }

    public DamageEvent OnShot;

    public void Shoot(float _damage, Vector3 _hitPos)
    {
        OnShot.Invoke(_damage, _hitPos);
    }
}