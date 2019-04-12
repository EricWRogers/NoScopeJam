using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    public GunType weapon;

    public override void OnPickedUp()
    {
        PlayerStats.Instance.OnGunTypeUnlocked(weapon.name);
    }
}
