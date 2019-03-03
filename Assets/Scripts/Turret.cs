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
    public GameObject ammoPickup, plasmaPickup, healthPickup;
    public LineRenderer laserTrail, laserTrail2;
    public float laserPassiveScale = .9f;
    public float laserActiveScale = .9f;
    
    public float maxHealth;
    private float currentHealth;
    private AudioSource audio;
    public Transform barrel1, barrel2, barrel3, barrel4;
    private int order;
    public float maxAgroTime = 5f;
    private float currentAgroTime = 0f;
    public Transform targetLaserEmit, sweepEmit, sweepEmit2;
    public Animator anim;
    public float searchSpeed = 2;
    public float targetRadius = 10;
    private Vector3 lastShotPosition = Vector3.zero; 


    private bool turretActive;
    private bool attacking;
    private bool canShoot = true;


    private void Start()
    {
        audio = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        currentAgroTime = 0;
        turretActive = false;

        if(GameManager.Instance != null)
        {
            target = GameManager.Instance.PlayerCurrentGO.transform;
            //RapidFire();
        }
        flare.gameObject.SetActive(false);
   
    }

    private void Update()
    {
        if((this.transform.position - target.position).magnitude <= targetRadius)
        {
            turretActive = true;
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        if (turretActive)
        {
            laserTrail2.enabled = false;
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
        audio.clip = currentGun.fireSFX;
        audio.Play();

        Debug.Log("Turret shoot");

        if (canShoot)
        {
            //anim.SetTrigger("Fire");
            currentBarrel.GetComponentInChildren<ParticleSystem>().Play();

            RaycastHit hit;
            // NameToLayer returns index. So, converting to it's bimask respresentation.
            int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

            // Combining playerLayerMask and FPSLayerMask ( | ), inverting them (~), and removing from a filled bit mask(1111111....11)
            int mask = int.MaxValue & ~(enemyLayerMask);


            Random.InitState(System.DateTime.Now.Millisecond);
            Vector3 direction = (target.position + new Vector3(Random.Range(.0f, 2f), Random.Range(.0f, 2f), Random.Range(.0f, 2f)) * Random.Range(0, 3f) - currentBarrel.position).normalized;
            Vector3 targetPos = currentBarrel.transform.position + (direction * currentGun.range);

            if(lastShotPosition == Vector3.zero)
            {
                lastShotPosition = targetPos;
            }

            Vector3 newPos = Vector3.Lerp(lastShotPosition, targetPos, laserActiveScale);

            lastShotPosition = Vector3.zero;

            if (Physics.Raycast(currentBarrel.transform.position, direction, out hit, currentGun.range, mask))
            {
                targetPos = hit.point;

                if (hit.collider.tag != "Enemy")
                {
                    lastShotPosition = newPos;

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

        attacking = false;
        CancelInvoke("FireGun");

        if(laserTrail2)
            laserTrail2.enabled = true;

        RaycastHit hit;
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        int mask = int.MaxValue & ~(enemyLayerMask);
        Vector3 direction = ((sweepEmit.forward * currentGun.range) - sweepEmit.transform.position).normalized;
        Vector3 targetPos = sweepEmit.transform.position + (direction * currentGun.range);

        Vector3 newDir = Vector3.Lerp(laserTrail.GetPosition(1) + (direction * currentGun.range), targetPos, laserPassiveScale);

        if (Physics.Raycast(sweepEmit.transform.position, newDir, out hit, currentGun.range, mask))
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
        
        Vector3 newPos = Vector3.Lerp(laserTrail.GetPosition(1), targetPos, laserPassiveScale);
        laserTrail.SetPosition(0, targetLaserEmit.position);
        laserTrail.SetPosition(1, newPos);


        RaycastHit hit2;
        Vector3 direction2 = ((sweepEmit2.forward * currentGun.range) - sweepEmit2.transform.position).normalized;
        Vector3 targetPos2 = sweepEmit2.transform.position + (direction2 * currentGun.range);

        Vector3 newDir2 = Vector3.Lerp(laserTrail2.GetPosition(1) + (direction2 * currentGun.range), targetPos2, laserPassiveScale);

        if (Physics.Raycast(sweepEmit2.transform.position, newDir2, out hit2, currentGun.range, mask))
        {
            targetPos2 = hit2.point;
            if (hit2.collider.tag == "Player")
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

        Vector3 newPos2 = Vector3.Lerp(laserTrail2.GetPosition(1), targetPos2, laserPassiveScale);
        laserTrail2.SetPosition(0, targetLaserEmit.position);
        laserTrail2.SetPosition(1, newPos2);
    }

    void LaserSight()
    {
        RaycastHit hit;

        // NameToLayer returns index. So, converting to it's bimask respresentation.
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        // Combining playerLayerMask and FPSLayerMask ( | ), inverting them (~), and removing from a filled bit mask(1111111....11)
        int mask = int.MaxValue & ~(enemyLayerMask);

        Vector3 direction = ((targetLaserEmit.forward * currentGun.range) - targetLaserEmit.transform.position).normalized;
        Vector3 targetPos = targetLaserEmit.transform.position + (direction * currentGun.range);

        Vector3 newDir = Vector3.Lerp(laserTrail.GetPosition(1) + (direction * currentGun.range), targetPos, laserPassiveScale);
        if (Physics.Raycast(targetLaserEmit.transform.position, newDir, out hit, currentGun.range, mask))
        {
            targetPos = hit.point;

            if(hit.collider.tag == "Player")
            {
                currentAgroTime = 0;
            }
        }

        Vector3 newPos = Vector3.Lerp(laserTrail.GetPosition(1), targetPos, laserPassiveScale);

        laserTrail.SetPosition(0, targetLaserEmit.position);
        laserTrail.SetPosition(1, newPos);

    }

    private void Attack()
    {
        if (!attacking)
        {
            RapidFire();
            attacking = true;
        }
        Movement(laserActiveScale);
    }

    void Sweep()
    {
        turretBase.Rotate(turretBase.transform.up, searchSpeed);
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

    public void SpawnPickups()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        for (int i = 0; i < 3; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-5,5), 0, Random.Range(-5, 5));

            int roll = Random.Range(0, 3);

            bool hasPlasmaGun = PlayerStats.Instance.UnlockedGuns.Contains("PlasmaRifle");

            switch (roll)
            {
                case 0:
                    var _ammoDrop = Instantiate(ammoPickup, turretBox.position + offset, this.transform.rotation) as GameObject;
                    break;
                case 1:
                    var _healthDrop = Instantiate(healthPickup, turretBox.position + offset, this.transform.rotation) as GameObject;
                    break;
                case 2:
                    if (hasPlasmaGun)
                    {
                        var _plasmaDrop = Instantiate(plasmaPickup, turretBox.position + offset, this.transform.rotation) as GameObject;
                    }
                    else
                    {
                        var _ammoReplaceDrop = Instantiate(ammoPickup, turretBox.position + offset, this.transform.rotation) as GameObject;
                    }
                    break;
                
            }
            
        }
    }

    public void Die()
    {
        var _fx = Instantiate(Resources.Load(deathFX.name), this.transform.position, this.transform.rotation) as GameObject;
        SpawnPickups();
        Destroy(this.gameObject);

    }
}
