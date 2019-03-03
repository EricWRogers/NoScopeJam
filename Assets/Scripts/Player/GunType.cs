using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GunType")]
public class GunType : ScriptableObject
{
    public enum Ammo
    {
        Bullets,
        Plasma,
        Grenades
    }

    public enum FiringMode
    {
        Single,
        Auto
    }
        
    public Ammo ammo;
    public Sprite image;
    public FiringMode mode;
    public GameObject hitFX;
    public GameObject trailFX;
    public AudioClip fireSFX;

    public float damage;
    public float range;
    public float plasmaSpeed;
    public float fireRate;
}
