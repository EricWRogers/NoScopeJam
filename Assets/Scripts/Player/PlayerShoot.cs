using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GunType currentGun;
    public Camera WeaponCamera;
    public Transform barrel;
    public ParticleSystem muzzleFlash;
    public Animator anim;
    private bool isAiming;
    public float aimingFOV;
    public float aimTime;
    private float originalFOV;

    private void Start()
    {
        originalFOV = Camera.main.fieldOfView;
    }

    private void Update()
    {
        WeaponCamera.fieldOfView = Camera.main.fieldOfView;

        AimEffects();

        if(CrossPlatformInputManager.GetButtonDown("Fire1"))
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
        if (CrossPlatformInputManager.GetButton("Aim"))
        {
            isAiming = true;
        }
        if(isAiming && CrossPlatformInputManager.GetButtonUp("Aim"))
        {
            isAiming = false;
        }

        
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
        anim.SetTrigger("Fire");
        muzzleFlash.Play();

        RaycastHit hit;
        // NameToLayer returns index. So, converting to it's bimask respresentation.
        int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        int FPSLayerMask = 1 << LayerMask.NameToLayer("FirstPerson");

        // Combining playerLayerMask and FPSLayerMask ( | ), inverting them (~), and removing from a filled bit mask(1111111....11)
        int mask = int.MaxValue & ~(playerLayerMask | FPSLayerMask);
        Debug.Log(System.Convert.ToString(mask, 2));
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, currentGun.range, mask))
        {
            Debug.Log(hit.collider.name);
            if(hit.collider.tag != "Player")
            {
                var _fx = Instantiate(Resources.Load(currentGun.hitFX.name), hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
                if(hit.collider.GetComponent<Shootable>() != null)
                {
                    hit.collider.GetComponent<Shootable>().Shoot(currentGun.damage, hit.point);
                }
            }
        }
    }

}
