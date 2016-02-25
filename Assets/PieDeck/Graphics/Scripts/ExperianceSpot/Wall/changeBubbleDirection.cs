using UnityEngine;
using System.Collections;

public class changeBubbleDirection : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Bubble")) {
			changeDirection (other);
		}
	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.CompareTag ("Bubble")) {
			changeDirection (other);
		}
	}

	void changeDirection(Collider other) {
		BoxCollider bc = GetComponent<BoxCollider>();
		Rigidbody rb = other.gameObject.GetComponent<Rigidbody> ();

		Vector3 bubblePos = other.transform.position; 
		//			Vector3 colliderPoint = bc.ClosestPointOnBounds (bubblePos); 
		Vector3 colliderPoint = other.bounds.ClosestPoint (Vector3.zero);  

		Vector3 direction = (colliderPoint - bubblePos);

		float force = rb.velocity.magnitude;
		if (force < 1)
			force = 1;
		//rb.velocity = Vector3.zero;
		rb.AddForce (direction.normalized * force * 2);
		//Debug.DrawRay(colliderPoint, direction.normalized * 1000, Color.green, 1000);
	}
}
