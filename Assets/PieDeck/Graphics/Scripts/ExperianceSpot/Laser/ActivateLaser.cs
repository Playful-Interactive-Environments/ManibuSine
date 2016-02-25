using UnityEngine;
using System.Collections;

public class ActivateLaser : MonoBehaviour {

	public GameObject laser;

	public GameObject[] bubbleColorContainer;

	void Start() {
		FeelingContainer.SetBubbleColorContainer (bubbleColorContainer);
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			laser.gameObject.SetActive (true);

			Material m = other.gameObject.GetComponent<Renderer> ().material;
			Renderer[] renderer =  transform.parent.gameObject.GetComponentsInChildren<Renderer> ();

			foreach (Renderer r in renderer) {
				r.material = m;
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			laser.gameObject.SetActive (false);
		}
	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			Vector3 relPos = transform.position - other.gameObject.transform.position;
			laser.transform.rotation = Quaternion.identity; 

			Vector3 rot = Vector3.zero;
			float maxDistance = transform.localScale.x / 2;

			if (relPos.z > maxDistance)
				rot.x = 0;
			else
				rot.x = -(relPos.z / maxDistance) * 60;

			if (relPos.x > maxDistance)
				rot.z = 0;
			else
				rot.z = (relPos.x / maxDistance) * 60;

			laser.transform.Rotate (rot);
		}
	}
}
