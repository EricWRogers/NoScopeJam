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
    public Animator playerAnim, graphicAnim;
    private AudioSource audio;
    private bool isAiming;
    private bool triggerDown = false;
    public float aimingFOV;
    public float aimTime;
    private float originalFOV;
    private bool canShoot;
    


    private void Start()
    {
        originalFOV = Camera.main.fieldOfView;
        audio = GetComponent<AudioSource>();

        if(currentGun.ammo == GunType.Ammo.Bullets)
        {
            if(currentGun.mode == GunType.FiringMode.Auto)
            {
                playerAnim.SetBool("Auto", true);
                playerAnim.SetBool("Single", false);
                playerAnim.SetBool("Plasma", false);
            }
            else
            {
                playerAnim.SetBool("Single", true);
                playerAnim.SetBool("Auto", false);
                playerAnim.SetBool("Plasma", false);
            }
        }
        else
        {
            playerAnim.SetBool("Plasma", true);
            playerAnim.SetBool("Auto", false);
            playerAnim.SetBool("Single", false);
        }
    }

    private void Update()
    {
        

        if (!UIManager.Instance.OutofMenu)
        {
            canShoot = false;
            return;
        }
        
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
        playerAnim.SetTrigger("Changing");

        GunType[] unlockedGuns = PlayerStats.Instance.UnlockedGunTypes.ToArray();

        gunIndex++;

        if(gunIndex >= unlockedGuns.Length)
        {
            gunIndex = 0;
        }

        currentGun = unlockedGuns[gunIndex];

        if(currentGun.mode == GunType.FiringMode.Single)
        {
            playerAnim.SetBool("Single", true);
            playerAnim.SetBool("Auto", false);
            playerAnim.SetBool("Plasma", false);
            //playerAnim.SetTrigger("Changing");
            // playerAnim.SetTrigger("Equip Single");
        }
        else
        {
            playerAnim.SetBool("Auto", true);
            playerAnim.SetBool("Single", false);
            playerAnim.SetBool("Plasma", false);
            // playerAnim.SetTrigger("Changing");
            //playerAnim.SetTrigger("Equip Auto");
        }

        if(currentGun.ammo == GunType.Ammo.Plasma)
        {
            playerAnim.SetBool("Plasma", true);
            playerAnim.SetBool("Single", false);
            playerAnim.SetBool("Auto", false);
        }

    }

    void AimEffects()
    {
        if (isAiming)
        {
            graphicAnim.SetBool("isAiming", true);
            float newFOV = Mathf.Lerp(Camera.main.fieldOfView, aimingFOV, aimTime);
            Camera.main.fieldOfView = newFOV;
        }
        else
        {
            graphicAnim.SetBool("isAiming", false);
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
            if(currentGun.ammo == GunType.Ammo.Bullets)
            {
                switch (currentGun.mode)
                {
                    case GunType.FiringMode.Auto:
                        AudioManager.Instance.SoundsEventTrigger(SoundEvents.ChainGun, false, audio);
                        break;
                    case GunType.FiringMode.Single:
                        AudioManager.Instance.SoundsEventTrigger(SoundEvents.Rifle, false, audio);
                        break;
                }

                PlayerStats.Instance.AddAmmoCount(GunType.Ammo.Bullets, -1);
                graphicAnim.SetTrigger("Fire");
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
        if(currentGun.ammo == GunType.Ammo.Plasma)
        {
            AudioManager.Instance.SoundsEventTrigger(SoundEvents.Railgun, false, audio);

            muzzleFlash.Play();
            PlayerStats.Instance.AddAmmoCount(GunType.Ammo.Bullets, -1);
            graphicAnim.SetTrigger("Fire");
            PlayerStats.Instance.AddAmmoCount(GunType.Ammo.Plasma, -1);

            Vector3 direction = (Camera.main.transform.forward).normalized;
            Vector3 targetPos = barrel.transform.position + (direction * currentGun.range);

            var _bullet = Instantiate((currentGun.projectile), barrel.position, Quaternion.LookRotation((barrel.forward - targetPos).normalized, barrel.transform.up), null) as GameObject;
            _bullet.gameObject.layer = 9;
            _bullet.GetComponent<FXDestroy>().destroyTime = currentGun.range;
            _bullet.GetComponent<Projectile>().direction = Camera.main.transform.forward.normalized;
            _bullet.GetComponent<Projectile>().speed = currentGun.plasmaSpeed;
            // _bullet.GetComponent<Rigidbody>().velocity = (Camera.main.transform.forward * currentGun.plasmaSpeed);
            _bullet.GetComponent<Projectile>().enemyBullet = false; 
            _bullet.GetComponent<Projectile>().hitFX = currentGun.hitFX;
            _bullet.GetComponent<Projectile>().damage = currentGun.damage;


        }
            
        
    }

}
