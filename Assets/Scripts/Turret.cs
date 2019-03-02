using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GunType currentGun;

    public Transform target;
    public Transform turretBase;

    public GameObject hitFX, deathFX;

    public float maxHealth;
    public float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;

        if(GameManager.Instance != null)
        {
            target = GameManager.Instance.PlayerCurrentGO.transform;
        }
    }

    private void Update()
    {
        if(currentHealth >= 0)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if(target != null)
        {
            turretBase.LookAt(target);
        }
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = currentHealth - _damage;
    }

    public void Die()
    {
        var _fx = Instantiate(Resources.Load(deathFX.name), this.transform.position, this.transform.rotation) as GameObject;

        Destroy(this.gameObject);

    }
}
