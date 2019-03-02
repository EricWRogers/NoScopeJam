using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
     public float Radius;
     public UnityEvent OnPickedUp;
     
     private GameObject _player;

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
//          if ()
     }

     private void OnTriggerEnter(Collider other)
     {
          if (other.CompareTag("Player"))
          {
               OnPickedUp.Invoke();
          }
     }
}