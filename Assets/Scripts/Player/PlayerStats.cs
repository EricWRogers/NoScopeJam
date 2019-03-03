using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    [Serializable]
    private class PlayerStatsData
    {
        public float health = 100;
        public int currentLevel;
        public Dictionary<GunType.Ammo, int> ammo = new Dictionary<GunType.Ammo, int>();
        public List<string> unlockedGuns = new List<string>();
    }

    public float healthRechargeRate = 20f;
    public float healthRechargeDelay = 3f;


    public float Health
    {
        get { return _playerStatsData.health; }
    }

    public List<string> UnlockedGuns
    {
        get { return _playerStatsData.unlockedGuns; }
    }

    public static PlayerStats Instance = null;

    [SerializeField] [ReadOnly] private PlayerStatsData _playerStatsData = new PlayerStatsData();
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
        }
    }

    public void Update()
    {
        if (_playerStatsData.health < 100f && Time.time > nextRechargableTime)
        {
            _playerStatsData.health += healthRechargeRate * Time.deltaTime;
        }
    }

    public void AddAmmoCount(GunType.Ammo ammoType, int ammoCount)
    {
        if (!_playerStatsData.ammo.ContainsKey(ammoType))
        {
            _playerStatsData.ammo[ammoType] = 0;
        }

        _playerStatsData.ammo[ammoType] += ammoCount;
    }

    public int GetAmmoCount(GunType.Ammo ammoType)
    {
        int ammoCount = 0;
        _playerStatsData.ammo.TryGetValue(ammoType, out ammoCount);

        return ammoCount;
    }

    public void UpdateHealth(float updateAmount)
    {
        if (updateAmount < 0)
        {
            nextRechargableTime = Time.time + healthRechargeDelay;
        }

        _playerStatsData.health += updateAmount;
        _playerStatsData.health = Mathf.Clamp(_playerStatsData.health, 0, 100);
    }

    public void OnNewLevelReached(int newLevel)
    {
        _playerStatsData.currentLevel = newLevel;
    }

    public void OnGunTypeUnlocked(string gunTypeName)
    {
        if (_playerStatsData.unlockedGuns.IndexOf(gunTypeName) < 0)
        {
            _playerStatsData.unlockedGuns.Add(gunTypeName);
        }
    }

    public void LoadFromJsonString(string json)
    {
        _playerStatsData = JsonConvert.DeserializeObject<PlayerStatsData>(json);
    }

    public string GetJsonString()
    {
        return JsonConvert.SerializeObject(_playerStatsData);
    }
}