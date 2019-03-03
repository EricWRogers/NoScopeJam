using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    private class PlayerStatsData
    {
        public float health;
        public Dictionary<GunType.Ammo, int> ammo = new Dictionary<GunType.Ammo, int>();
    }

    public float Health
    {
        get { return _playerStatsData.health; }
    }

    public static PlayerStats Instance = null;

    private PlayerStatsData _playerStatsData = new PlayerStatsData();

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
        _playerStatsData.health += updateAmount;
        _playerStatsData.health = Mathf.Clamp(_playerStatsData.health, 0, 100);
    }

    public string GetJsonString()
    {
        return JsonConvert.SerializeObject(_playerStatsData);
    }
}