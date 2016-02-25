using UnityEngine;
using System.Collections;

public class PickEmotion : MonoBehaviour {

	public GameObject NetworkCube;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			Material m = gameObject.GetComponent<Renderer> ().material;

			if (NetworkCube != null)
				NetworkCube.GetComponent<Renderer> ().material = m;
			//other.GetComponent<Renderer> ().material = m;

			Transform[] child = other.GetComponentsInChildren<Transform> ();
			foreach (Transform c in child) {
				Renderer r = c.gameObject.GetComponent<Renderer> ();
				if (r != null)
					r.material = m;
			}

			Destroy (gameObject);
		}
	}
}
