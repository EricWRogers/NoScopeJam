using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GunType currentGun;

    private Transform target;
    public Transform turretBase, turretBox;

    public GameObject hitFX, deathFX;
    public GameObject flare;
    public LineRenderer laserTrail;
    public float laserPassiveScale = .9f;
    public float laserActiveScale = .9f;

    public float maxHealth;
    public float currentHealth;

    public Transform barrel1, barrel2, barrel3, barrel4;
    private int order;
    public float maxAgroTime = 5f;
    private float currentAgroTime = 0f;
    public Transform targetLaserEmit;
    public Animator anim;


    private bool turretActive;
    private bool canShoot;


    private void Start()
    {
        currentHealth = maxHealth;
        currentAgroTime = 0;
        turretActive = false;

        if(GameManager.Instance != null)
        {
            target = GameManager.Instance.PlayerCurrentGO.transform;
            Debug.Log(currentGun.name + " " + currentGun.damage);
            //RapidFire();
        }
        flare.gameObject.SetActive(false);
   
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }

        if (turretActive)
        {
            Attack();
            currentAgroTime += Time.deltaTime;

            if (currentAgroTime >= maxAgroTime)
            {
                Debug.Log("Gave Up");
                currentAgroTime = 0;
                turretActive = false;
            }

        }
        else
        {
            PassiveScan();
        }


    }

    void Shoot(Transform currentBarrel)
    {
        if (canShoot)
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


    private void PassiveScan()
    {
        Sweep();

        RaycastHit hit;

        // NameToLayer returns index. So, converting to it's bimask respresentation.
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        // Combining playerLayerMask and FPSLayerMask ( | ), inverting them (~), and removing from a filled bit mask(1111111....11)
        int mask = int.MaxValue & ~(enemyLayerMask);

        Vector3 direction = ((targetLaserEmit.forward * currentGun.range) - targetLaserEmit.transform.position).normalized;
        Vector3 targetPos = targetLaserEmit.transform.position + (direction * currentGun.range);

        Vector3 newDir = Vector3.Lerp(laserTrail.GetPosition(1) + (direction * currentGun.range), targetPos, laserPassiveScale);

        laserTrail.SetPosition(0, targetLaserEmit.position);
        laserTrail.SetPosition(1, newDir);



        if (Physics.Raycast(targetLaserEmit.transform.position, newDir, out hit, currentGun.range, mask))
        {
            targetPos = hit.point;

            if (hit.collider.tag == "Player")
            {
                Debug.Log("Turret Found Player");
                flare.gameObject.SetActive(true);
                currentAgroTime = 0;
                turretActive = true;
            }
            else
            {
                flare.gameObject.SetActive(false);
            }

        }
    }

    void LaserSight()
    {
        Vector3 direction = ((targetLaserEmit.forward * currentGun.range) - targetLaserEmit.transform.position).normalized;
        Vector3 targetPos = targetLaserEmit.transform.position + (direction * currentGun.range);

        Vector3 newDir = Vector3.Lerp(laserTrail.GetPosition(1) + (direction * currentGun.range), targetPos, laserPassiveScale);

        laserTrail.SetPosition(0, targetLaserEmit.position);
        laserTrail.SetPosition(1, newDir);
    }

    private void Attack()
    {
        Debug.Log("Attacking");
        Movement(laserActiveScale);
    }

    void Sweep()
    {
        turretBase.Rotate(turretBase.transform.up, 2);
    }

    void Movement(float _scale)
    {
        LaserSight();

        if(target != null)
        {
            //turretBase.LookAt(target, turretBase.transform.up);

            //turretBase.rotation = Quaternion.FromToRotation(turretBase.forward, (turretBase.position - target.position).normalized);
            Quaternion lookYRot = Quaternion.LookRotation(target.position - turretBase.position, turretBase.transform.up);
            //Vector3 currentYRot = turretBase.localRotation.eulerAngles;
            lookYRot.x = 0;
            lookYRot.z = 0;
           // new Vector3 newYRot = Vector3.Lerp(currentYRot, );
            turretBase.localRotation = Quaternion.Lerp(turretBase.localRotation, lookYRot, _scale);


            Quaternion lookXRot = Quaternion.LookRotation(target.position - turretBox.position, turretBox.transform.up);
            //Vector3 currentYRot = turretBase.localRotation.eulerAngles;
            lookXRot.y = 0;
            lookXRot.z = 0;
            // new Vector3 newYRot = Vector3.Lerp(currentYRot, );
            turretBox.localRotation = Quaternion.Lerp(turretBox.localRotation, lookXRot, _scale);
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
