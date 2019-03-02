using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health;
    public float ammo;

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
}