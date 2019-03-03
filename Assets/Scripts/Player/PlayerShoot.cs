using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GunType currentGun;
    private int gunIndex = 0;

    public Camera WeaponCamera;
    public Transform barrel;
    public ParticleSystem muzzleFlash;
    public Animator anim;
    private bool isAiming;
    private bool triggerDown = false;
    public float aimingFOV;
    public float aimTime;
    private float originalFOV;
    private bool canShoot;
    


    private void Start()
    {
        originalFOV = Camera.main.fieldOfView;
    }

    private void Update()
    {
        WeaponCamera.fieldOfView = Camera.main.fieldOfView;

        AimEffects();

        switch (currentGun.ammo)
        {
            case GunType.Ammo.Bullets:
                if (PlayerStats.Instance.GetAmmoCount(GunType.Ammo.Bullets) <= 0 )
                {
                    canShoot = false;
                }
                else
                {
                    canShoot = true;
                }
                break;
            case GunType.Ammo.Plasma:
                if (PlayerStats.Instance.GetAmmoCount(GunType.Ammo.Plasma) <= 0)
                {
                    canShoot = false;
                }
                else
                {
                    canShoot = true;
                }
                break;
        }



        if (CrossPlatformInputManager.GetAxis("Fire1") > 0.25)
        {
            if (!triggerDown)
            {
                switch (currentGun.mode)
                {
                    case GunType.FiringMode.Auto:
                        RapidFire();
                        break;
                    case GunType.FiringMode.Single:
                        Shoot();
                        break;
                }

                triggerDown = true;
            }
        }
        if (CrossPlatformInputManager.GetAxis("Fire1") < 0.25)
        {
            if (triggerDown)
            {
                CancelInvoke("Shoot");
                triggerDown = false;
            }
        }
        

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            switch (currentGun.mode)
            {
                case GunType.FiringMode.Auto:
                    RapidFire();
                    break;
                case GunType.FiringMode.Single:
                    Shoot();
                    break;
            }
        }
        if (CrossPlatformInputManager.GetButtonUp("Fire1"))
        {
            CancelInvoke("Shoot");
        }
        if (CrossPlatformInputManager.GetButton("Aim") || CrossPlatformInputManager.GetAxis("Fire2") > 0.25f)
        {
            isAiming = true;
        }
        if(isAiming && CrossPlatformInputManager.GetButtonUp("Aim") || CrossPlatformInputManager.GetAxis("Fire2") < 0.25f)
        {
            isAiming = false;
        }

        if(CrossPlatformInputManager.GetButtonUp("Swap Weapon"))
        {
            SwitchWeapon();
        }

    }

    void SwitchWeapon()
    {
        GunType[] unlockedGuns = PlayerStats.Instance.UnlockedGunTypes.ToArray();

        gunIndex++;

        if(gunIndex >= unlockedGuns.Length)
        {
            gunIndex = 0;
        }

        currentGun = unlockedGuns[gunIndex];

    }

    void AimEffects()
    {
        if (isAiming)
        {
            anim.SetBool("isAiming", true);
            float newFOV = Mathf.Lerp(Camera.main.fieldOfView, aimingFOV, aimTime);
            Camera.main.fieldOfView = newFOV;
        }
        else
        {
            anim.SetBool("isAiming", false);
            float newFOV = Mathf.Lerp(Camera.main.fieldOfView, originalFOV, aimTime);
            Camera.main.fieldOfView = newFOV;
        }
    }

    void RapidFire()
    {
        Debug.Log("Rapid Fire");
        InvokeRepeating("Shoot", Time.deltaTime, currentGun.fireRate);
    }
    
    void Shoot()
    {
        if (canShoot)   
        {
            PlayerStats.Instance.AddAmmoCount(GunType.Ammo.Bullets, -1);
            anim.SetTrigger("Fire");
            muzzleFlash.Play();

            RaycastHit hit;
            // NameToLayer returns index. So, converting to it's bimask respresentation.
            int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
            int FPSLayerMask = 1 << LayerMask.NameToLayer("FirstPerson");

            // Combining playerLayerMask and FPSLayerMask ( | ), inverting them (~), and removing from a filled bit mask(1111111....11)


            Vector3 direction = (Camera.main.transform.forward).normalized;
            Vector3 targetPos = barrel.transform.position + (direction * currentGun.range);

            int mask = int.MaxValue & ~(playerLayerMask | FPSLayerMask);
            if (Physics.Raycast(barrel.transform.position, Camera.main.transform.forward, out hit, currentGun.range, mask))
            {
                targetPos = hit.point;

                if (hit.collider.tag != "Player")
                {
                    if (hit.collider.tag != "Enemy")
                    {
                        var _fx = Instantiate(Resources.Load(currentGun.hitFX.name), hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
                    }
                    if (hit.collider.GetComponent<Shootable>() != null)
                    {
                        hit.collider.GetComponent<Shootable>().Shoot(currentGun.damage, hit.point);
                    }
                }
            }

            var _linefx = Instantiate(Resources.Load(currentGun.trailFX.name), barrel.transform.position, barrel.transform.rotation) as GameObject;
            _linefx.GetComponent<LineRenderer>().SetPosition(0, barrel.position);
            _linefx.GetComponent<LineRenderer>().SetPosition(1, targetPos);
        }
        
    }

}
