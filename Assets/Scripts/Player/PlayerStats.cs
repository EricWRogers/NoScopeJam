using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    public float health { get; private set; }
    private Dictionary<GunType.Ammo, int> ammo = new Dictionary<GunType.Ammo, int>();

    public static PlayerStats Instance = null;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void AddAmmoCount(GunType.Ammo ammoType, int ammoCount)
    {
        if (!ammo.ContainsKey(ammoType))
        {
            ammo[ammoType] = 0;
        }

        ammo[ammoType] += ammoCount;
    }

    public int GetAmmoCount(GunType.Ammo ammoType)
    {
        int ammoCount = 0;
        ammo.TryGetValue(ammoType, out ammoCount);

        return ammoCount;
    }

    public void UpdateHealth(float updateAmount)
    {
        health += updateAmount;
        health = Mathf.Clamp(health, 0, 100);
    }
}