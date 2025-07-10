using System;
using System.Collections;
using UnityEngine;

public class Throwable : MonoBehaviour
{
   private GameManager _gameManger;
   [SerializeField]private float forceAmount = 300f;
   public Vector3 forceDirection = Vector3.back;

   private void Start()
   {
      forceAmount = 5000f;
      Rigidbody rb = GetComponent<Rigidbody>();
      if (rb != null)
      {
         rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
      }
   }

   private void Update()
   {
      Debug.DrawRay(transform.position, GetComponent<Rigidbody>().linearVelocity.normalized * 5f, Color.yellow);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         Rigidbody playerRb = other.GetComponent<Rigidbody>();

         if (playerRb != null)
         {
            Vector3 pushDirection = (other.transform.position - transform.position).normalized;
            StartCoroutine(ApplyForceGradually(playerRb, pushDirection, forceAmount, 0.5f));
         }
         StartCoroutine(DeleteAfterCollision());
      }
      if (other.CompareTag("Ground"))
      {
         Destroy(gameObject);
      }
      StartCoroutine(DeleteItem());
   }

   private IEnumerator ApplyForceGradually(Rigidbody rb, Vector3 direction, float totalForce, float duration)
   {
      float elapsed = 0f;
      float forcePerFrame = totalForce / (duration / Time.fixedDeltaTime);

      while (elapsed < duration)
      {
         rb.AddForce(direction * forcePerFrame, ForceMode.Force);
         elapsed += Time.fixedDeltaTime;
         yield return new WaitForFixedUpdate();
      }
   }

   private IEnumerator DeleteAfterCollision()
   {
      yield return new WaitForSeconds(0.6f);
      Destroy(this.gameObject);   
   }
   private IEnumerator DeleteItem()
   {
      yield return new WaitForSeconds(2f);
      Destroy(this.gameObject);
   }
}
