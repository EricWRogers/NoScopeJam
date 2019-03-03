using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerModel : MonoBehaviour
{
    public Camera fpsCamera;

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TakeDamage(float _damage, Vector3 _pos)
    {
        Debug.Log("Player Took Damage");

        PlayerStats.Instance.UpdateHealth(-_damage);

        float currentHealth = PlayerStats.Instance.Health;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    

}
