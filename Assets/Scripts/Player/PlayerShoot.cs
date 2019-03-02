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
            Shoot();
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

    void Shoot()
    {
        anim.SetTrigger("Fire");
        muzzleFlash.Play();

        RaycastHit hit;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, currentGun.range, 9))
        {
            if(hit.collider.tag != "Player")
            {
                var _fx = Instantiate(Resources.Load(currentGun.hitFX.name), hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;

            }
        }
    }

}
