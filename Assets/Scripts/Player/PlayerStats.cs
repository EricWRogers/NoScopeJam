using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    [Serializable]
    public class PlayerStatsData
    {
        public float health = 100;
        public int currentLevel = 0;
        public Dictionary<GunType.Ammo, int> ammo = new Dictionary<GunType.Ammo, int>();
        public List<string> unlockedGuns = new List<string>();

        public void reset()
        {
            health = 100;
            ammo.Clear();
            unlockedGuns.Clear();

            ammo[GunType.Ammo.Bullets] = 100;
            unlockedGuns.Add(PlayerStats.Instance.GetGunType("Gatling").name);
        }
    }

    public float healthRechargeRate = 20f;
    public float healthRechargeDelay = 3f;


    public float Health
    {
        get { return playerStatsData.health; }
    }

    public float ThrusterCharge
    {
        get
        {
            if (GameManager.Instance.PlayerCurrentGO)
            {
                return GameManager.Instance.PlayerCurrentGO.GetComponent<CustomFirstPersonController>()
                    .ThrusterChargeLeft;
            }

            return 0;
        }
    }

    public int CurrentAmmo
    {
        get
        {
            if (GameManager.Instance.PlayerCurrentGO)
            {
                GunType gunType = CurrentGun;
                if (gunType)
                {
                    return GetAmmoCount(gunType.ammo);
                }
            }

            return 0;
        }
    }

    public GunType CurrentGun
    {
        get
        {
            if (GameManager.Instance.PlayerCurrentGO)
            {
                return GameManager.Instance.PlayerCurrentGO.GetComponent<PlayerShoot>().currentGun;
            }

            return null;
        }
    }

    public List<string> UnlockedGuns
    {
        get { return playerStatsData.unlockedGuns; }
    }

    public List<GunType> UnlockedGunTypes
    {
        get
        {
            List<GunType> gunTypes = new List<GunType>();
            foreach (string gunName in playerStatsData.unlockedGuns)
            {
                gunTypes.Add(GetGunType(gunName));
            }

            return gunTypes;
        }
    }

    public static PlayerStats Instance = null;

    [ReadOnly] public PlayerStatsData playerStatsData = new PlayerStatsData();
    private CustomFirstPersonController _customFirstPersonController;
    private float nextRechargableTime = float.MinValue;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public void Update()
    {
        if (playerStatsData.health < 100f && Time.time > nextRechargableTime)
        {
            UpdateHealth(healthRechargeRate * Time.deltaTime);
        }
    }

    public void AddAmmoCount(GunType.Ammo ammoType, int ammoCount)
    {
        if (!playerStatsData.ammo.ContainsKey(ammoType))
        {
            playerStatsData.ammo[ammoType] = 0;
        }

        playerStatsData.ammo[ammoType] += ammoCount;
    }

    public int GetAmmoCount(GunType.Ammo ammoType)
    {
        int ammoCount = 0;
        playerStatsData.ammo.TryGetValue(ammoType, out ammoCount);

        return ammoCount;
    }

    public void UpdateHealth(float updateAmount)
    {
        if (updateAmount < 0)
        {
            nextRechargableTime = Time.time + healthRechargeDelay;
        }

        playerStatsData.health += updateAmount;
        playerStatsData.health = Mathf.Clamp(playerStatsData.health, 0, 100);
    }

    public void OnNewLevelReached(int newLevel)
    {
        playerStatsData.currentLevel = newLevel;
    }

    public GunType GetGunType(string name)
    {
        foreach (GunType gunType in GameManager.Instance.GunTypes)
        {
            if (gunType.name == name)
            {
                return gunType;
            }
        }

        return null;
    }

    public void OnGunTypeUnlocked(string gunTypeName)
    {
        if (playerStatsData.unlockedGuns.IndexOf(gunTypeName) < 0)
        {
            playerStatsData.unlockedGuns.Add(gunTypeName);
        }
    }

    public void LoadFromJsonString(string json)
    {
        playerStatsData = JsonConvert.DeserializeObject<PlayerStatsData>(json);
    }

    public string GetJsonString()
    {
        return JsonConvert.SerializeObject(playerStatsData);
    }
}