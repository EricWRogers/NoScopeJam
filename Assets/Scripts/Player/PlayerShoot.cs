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
    private float originalFOV;

    private void Start()
    {
        originalFOV = Camera.main.fieldOfView;
    }

    private void Update()
    {
        WeaponCamera.fieldOfView = Camera.main.fieldOfView;

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

        if (isAiming)
        {
            anim.SetBool("isAiming", true);
            Camera.main.fieldOfView = aimingFOV;
        }
        else
        {
            anim.SetBool("isAiming", false);
            Camera.main.fieldOfView = originalFOV;
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hit;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, currentGun.range))
        {
            var _fx = Instantiate(Resources.Load(currentGun.hitFX.name), hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
        }
    }

}
