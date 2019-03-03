using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GunType currentGun;

    public Transform target;
    public Transform turretBase, turretBox;

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
        if(currentHealth <= 0)
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
            turretBase.LookAt(target, turretBase.transform.up);
            //turretBase.rotation = Quaternion.FromToRotation(turretBase.forward, (turretBase.position - target.position).normalized);
            Vector3 currentYRot = turretBase.localRotation.eulerAngles;
            Debug.Log(currentYRot);
            currentYRot.x = 0;
            currentYRot.z = 0;
            turretBase.localRotation = Quaternion.Euler(currentYRot);

            turretBase.LookAt(target, turretBase.transform.up);
            //turretBase.rotation = Quaternion.FromToRotation(turretBase.forward, (turretBase.position - target.position).normalized);
            Vector3 currentXRot = turretBase.localRotation.eulerAngles;
            Debug.Log(currentXRot);
            currentXRot.y = 0;
            currentXRot.z = 0;
            turretBox.localRotation = Quaternion.Euler(currentXRot);




        }
    }

    public void TakeDamage(float _damage, Vector3 _hitPos)
    {
        Debug.Log("turret took damage");
        var _fx = Instantiate(Resources.Load(hitFX.name), _hitPos, hitFX.transform.rotation) as GameObject;
        currentHealth = currentHealth - _damage;
    }

    public void Die()
    {
        var _fx = Instantiate(Resources.Load(deathFX.name), this.transform.position, this.transform.rotation) as GameObject;

        Destroy(this.gameObject);

    }
}
