using UnityEngine;

public class AmmoPickup : Pickup
{
    public GunType.Ammo ammoType = GunType.Ammo.Bullets;
    public int ammoCount;

    public override void OnPickedUp()
    {
        PlayerStats.Instance.AddAmmoCount(ammoType, ammoCount);
    }
}