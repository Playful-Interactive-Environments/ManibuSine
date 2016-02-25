using UnityEngine;
using System.Collections;

public class ActivateExperianceSpot : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			other.gameObject.GetComponent<DrawFootprint> ().enabled = false;
			other.gameObject.GetComponent<CreateWall> ().enabled = false;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			other.gameObject.GetComponent<DrawFootprint> ().enabled = true;
			other.gameObject.GetComponent<CreateWall> ().enabled = true;
		}
	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			other.gameObject.GetComponent<DrawFootprint> ().enabled = false;
			other.gameObject.GetComponent<CreateWall> ().enabled = false;
		}
	}
}
