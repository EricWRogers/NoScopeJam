using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerModel : MonoBehaviour
{
    public Camera fpsCamera;
    public CustomFirstPersonController player;
    public AudioSource thrusterSource;
    public PlayerShoot playerShoot;
    public GlitchEffect glitchEffect;

    private bool playedThrustSound;

    private void Awake()
    {
        glitchEffect = GetComponentInChildren<GlitchEffect>();
    }

    private void Start()
    {
        PlayerStats.Instance._playerModel = this;
        playerShoot = GetComponent<PlayerShoot>();
        player = GetComponent<CustomFirstPersonController>();
    }

    private void Update()
    {
        playerShoot.graphicAnim.SetBool("Thruster", player.ref_isUsingThrusters());

        playerShoot.graphicAnim.SetBool("Running", player.ref_isRunning());

        playerShoot.graphicAnim.SetBool("Walking", player.ref_isWalking());

        if (player.ref_isUsingThrusters() && !playedThrustSound)
        {
            thrusterSource.Play();
            playedThrustSound = true;
        }
        if(!player.ref_isUsingThrusters())
        {
            thrusterSource.Stop();
            playedThrustSound = false;
        }
    }

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