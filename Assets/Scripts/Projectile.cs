using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public bool enemyBullet = false;

    public GameObject hitFX;

    public float damage;
    public Vector3 direction;
    public float speed;
    

    private void Update()
    {
        //ransform.Translate((direction)  * speed);
        Vector3 newPos = transform.position;

        newPos += direction * Time.deltaTime * speed;

        transform.position = newPos;

        /*
        if (enemyBullet)
        {
            RaycastHit hit;

            // NameToLayer returns index. So, converting to it's bimask respresentation.
            int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

            // Combining playerLayerMask and FPSLayerMask ( | ), inverting them (~), and removing from a filled bit mask(1111111....11)
            int mask = int.MaxValue & ~(enemyLayerMask);

            if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward), out hit, 1, mask) || Physics.Raycast(this.transform.position, Vector3.down, out hit, 1, mask))
            {
                if (hit.collider.tag != "Enemy")
                {
                    if (hit.collider.tag != "Player")
                    {
                        var _fx = Instantiate(hitFX, hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
                    }
                    if (hit.collider.GetComponent<Shootable>() != null)
                    {
                        hit.collider.GetComponent<Shootable>().Shoot(damage, hit.point);
                    }
                }

                Destroy(this.gameObject);
            }
        }
        else
        {
            RaycastHit hit;

            int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
            int FPSLayerMask = 1 << LayerMask.NameToLayer("FirstPerson");

            int mask = int.MaxValue & ~(playerLayerMask | FPSLayerMask);

            if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward), out hit, 1, mask) || Physics.Raycast(this.transform.position, Vector3.down, out hit, 1, mask))
            {
                if (hit.collider.tag != "Player")
                {
                    if (hit.collider.tag != "Enemy")
                    {
                        var _fx = Instantiate(hitFX, hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
                    }
                    if (hit.collider.GetComponent<Shootable>() != null)
                    {
                        hit.collider.GetComponent<Shootable>().Shoot(damage, hit.point);
                    }
                }

                Destroy(this.gameObject);
            }
        }
        */
        
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (enemyBullet)
        {
            if (col.tag != "Enemy" && col.tag != "Projectile")
            {
                Debug.Log(col.name);
                var _fx = Instantiate(hitFX, this.transform.position, this.transform.rotation) as GameObject;

                if (col.GetComponent<Shootable>() != null)
                {
                    col.GetComponent<Shootable>().Shoot(damage, this.transform.position);
                }

                Destroy(this.gameObject);
            }
        }
        if (!enemyBullet)
        {
            if (col.tag != "Player" && col.tag != "Projectile")
            {
                Debug.Log(col.name);
                var _fx = Instantiate(hitFX, this.transform.position, this.transform.rotation) as GameObject;

                if (col.GetComponent<Shootable>() != null)
                {
                    col.GetComponent<Shootable>().Shoot(damage, this.transform.position);
                }

                Destroy(this.gameObject);
            }
        }

    }

}
