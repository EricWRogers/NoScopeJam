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
    public Transform targetLaser;
    public Animator anim;

    private void Start()
    {
        currentHealth = maxHealth;

        if(GameManager.Instance != null)
        {
            target = GameManager.Instance.PlayerCurrentGO.transform;
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
        Debug.Log(System.Convert.ToString(mask, 2));

        Vector3 direction = (GameManager.Instance.PlayerCurrentGO.transform.position - currentBarrel.transform.position).normalized;

        Vector3 targetPos = currentBarrel.transform.position + (direction * currentGun.range);


        if (Physics.Raycast(currentBarrel.transform.position, direction, out hit, currentGun.range, mask))
        {
            targetPos = hit.point;

            Debug.Log(hit.collider.name);
            if (hit.collider.tag != "Enemy")
            {
                var _fx = Instantiate(Resources.Load(currentGun.hitFX.name), hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
                if (hit.collider.GetComponent<Shootable>() != null)
                {
                    hit.collider.GetComponent<Shootable>().Shoot(currentGun.damage, hit.point);
                }
            }
        }

        var _linefx = Instantiate(Resources.Load(currentGun.trailFX.name), currentBarrel.transform.position, currentBarrel.transform.rotation) as GameObject;
        _linefx.GetComponent<LineRenderer>().SetPosition(0, currentBarrel.position);
        _linefx.GetComponent<LineRenderer>().SetPosition(1, targetPos);
    }

    void RapidFire()
    {
        InvokeRepeating("FireAllGuns", Time.deltaTime, currentGun.fireRate);
    }

    void FireAllGuns()
    {
        Shoot(barrel1);
        Shoot(barrel2);
        Shoot(barrel3);
        Shoot(barrel4);
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
        //
        Destroy(this.gameObject);

    }
}
