using UnityEngine;
using UnityEngine.Events;

public abstract class Pickup : MonoBehaviour
{
     public float Radius = 30;
     public float FollowSpeed = 2;
     public UnityEvent OnPickedUpEvent;
     
     private GameObject _player;
     private bool followPlayer = false;

     private void Start()
     {
          _player = GameManager.Instance.PlayerCurrentGO;
     }
     
     private void OnDrawGizmos()
     {
          Gizmos.color = Color.red;
          
          Gizmos.DrawWireSphere(transform.position, Radius);
     }

     private void Update()
     {
          Debug.Log("Dist: " + Vector3.Distance(transform.position, _player.transform.position));
          if (_player && Vector3.Distance(transform.position, _player.transform.position) < Radius)
          {
               followPlayer = true;
          }

          if (followPlayer)
          {
               Debug.Log("Following Player");
               transform.position = Vector3.Lerp(transform.position, _player.transform.position, Time.deltaTime * FollowSpeed);
          }
     }

     private void OnTriggerEnter(Collider other)
     {
          if (other.CompareTag("Player"))
          {
               OnPickedUp();
               OnPickedUpEvent.Invoke();
               Destroy(this.gameObject);
          }
     }

     public abstract void OnPickedUp();
}