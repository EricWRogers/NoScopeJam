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

    public Transform barrel1, barrel2, barrel3, barrel4;
    public int order;
    public Transform targetLaser;
    public Animator anim;

    private void Start()
    {
        currentHealth = maxHealth;

        if(GameManager.Instance != null)
        {
            target = GameManager.Instance.PlayerCurrentGO.transform;
            Debug.Log(currentGun.name + " " + currentGun.damage);
            RapidFire();
        }
    }

    void Shoot(Transform currentBarrel)
    {
        //anim.SetTrigger("Fire");
       currentBarrel.GetComponentInChildren<ParticleSystem>().Play();

        RaycastHit hit;
        // NameToLayer returns index. So, converting to it's bimask respresentation.
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        // Combining playerLayerMask and FPSLayerMask ( | ), inverting them (~), and removing from a filled bit mask(1111111....11)
        int mask = int.MaxValue & ~(enemyLayerMask);

        Vector3 direction = (GameManager.Instance.PlayerCurrentGO.transform.position - currentBarrel.transform.position).normalized;

        Vector3 targetPos = currentBarrel.transform.position + (direction * currentGun.range);


        if (Physics.Raycast(currentBarrel.transform.position, direction, out hit, currentGun.range, mask))
        {
            targetPos = hit.point;
            
            if (hit.collider.tag != "Enemy")
            {
                var _fx = Instantiate(Resources.Load(currentGun.hitFX.name), hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
                if (hit.collider.GetComponent<Shootable>() != null)
                {
                    hit.collider.GetComponent<Shootable>().Shoot(currentGun.damage, hit.point);
                    Debug.Log(currentGun.damage);
                }
            }   
        }

        var _linefx = Instantiate(Resources.Load(currentGun.trailFX.name), currentBarrel.transform.position, currentBarrel.transform.rotation) as GameObject;
        _linefx.GetComponent<LineRenderer>().SetPosition(0, currentBarrel.position);
        _linefx.GetComponent<LineRenderer>().SetPosition(1, targetPos);
    }

    void RapidFire()
    {
        InvokeRepeating("FireGun", 0f, currentGun.fireRate);
    }

    void FireGun()
    {
        switch (order)
        {
            case 1:
                Shoot(barrel1);
                break;
            case 2:
                Shoot(barrel2);
                break;
            case 3:
                Shoot(barrel3);
                break;
            case 4:
                Shoot(barrel4);
                break;

        }

        order++;

        if(order >= 5)
        {
            order = 0;
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
            currentYRot.x = 0;
            currentYRot.z = 0;
            turretBase.localRotation = Quaternion.Euler(currentYRot);

            turretBase.LookAt(target, turretBase.transform.up);
            //turretBase.rotation = Quaternion.FromToRotation(turretBase.forward, (turretBase.position - target.position).normalized);
            Vector3 currentXRot = turretBase.localRotation.eulerAngles;
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
