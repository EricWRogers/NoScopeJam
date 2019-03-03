using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerModel : MonoBehaviour
{
    public Camera fpsCamera;

    void Die()
    {
        
        // Handled by PlayerStats Now
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TakeDamage(float _damage, Vector3 _pos)
    {
        Debug.Log("Player Took Damage");

        PlayerStats.Instance.UpdateHealth(-_damage);

        float currentHealth = PlayerStats.Instance.Health;

        /*if(currentHealth <= 0)
        {
            Die();
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WinTrigger"))
        {
            PlayerStats.Instance.BroadcastGameOverEvent(true);
        }
    }
}
