using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GunType currentGun;
    public Transform barrel;
    


    private void Update()
    {
        if(CrossPlatformInputManager.GetButtonDown("Fire 1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, currentGun.range))
        {

        }
    }

}
