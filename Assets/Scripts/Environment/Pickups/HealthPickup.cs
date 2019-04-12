using UnityEngine;

public class HealthPickup : Pickup
{
    public float health = 10f;

    public override void OnPickedUp()
    {
        PlayerStats.Instance.UpdateHealth(health);
    }
}