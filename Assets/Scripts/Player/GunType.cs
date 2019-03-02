using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GunType")]
public class GunType : ScriptableObject
{
    public enum Ammo
    {
        Bullets,
        Plasma
    }

    public enum FiringMode
    {
        Single,
        Auto
    }

    public Ammo ammo;
    public FiringMode mode;

    public int damage;
    public float range;
}
