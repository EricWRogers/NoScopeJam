using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
     public float Radius;
     public float FollowSpeed;
     public UnityEvent OnPickedUp;
     
     private GameObject _player;
     private bool followPlayer = false;

     private void Start()
     {
          _player = GameManager.Instance.PlayerPrefabGO;
     }
     
     private void OnDrawGizmos()
     {
          Gizmos.color = Color.red;
          
          Gizmos.DrawWireSphere(transform.position, Radius);
     }

     private void Update()
     {
          if (_player && Vector3.Distance(transform.position, _player.transform.position) < Radius)
          {
               followPlayer = true;
          }

          if (followPlayer)
          {
               transform.position = Vector3.Lerp(transform.position, _player.transform.position, Time.deltaTime * FollowSpeed);
          }
     }

     private void OnTriggerEnter(Collider other)
     {
          if (other.CompareTag("Player"))
          {
               OnPickedUp.Invoke();
               Destroy(this.gameObject);
          }
     }
}