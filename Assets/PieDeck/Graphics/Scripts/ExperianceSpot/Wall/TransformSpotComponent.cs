using UnityEngine;
using System.Collections;

public class TransformSpotComponent : MonoBehaviour {

	public GameObject player;
	public bool isActive = false;
	public bool isMoving = false;

	void OnTriggerStay (Collider other) {

		if (other.gameObject.CompareTag("Player")) {
			isActive = true;
			isMoving = false;

			player = other.gameObject;
			Vector3 direction = player.GetComponent<PlayerMovement> ().movingDirection;

			RaycastHit hit;
			if (Physics.Raycast (player.transform.position, direction, out hit, 10.0F)) {
				

				if (hit.transform.gameObject.CompareTag ("TransformSpot")) {
					transform.parent.position += direction;
					isMoving = true;

				} 
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			isActive = false;
		}
	}
}
