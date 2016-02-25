using UnityEngine;
using System.Collections;

public class OctaveComponent : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			Material m = gameObject.GetComponent<Renderer> ().material;
			GameObject container = m.GetBubbleColorContainer ();

			if (container != null) {
				Transform[] bubbles = container.GetComponentsInChildren<Transform> ();
				foreach (Transform bubble in bubbles) {
					DeformBubbleComponent dbc = bubble.gameObject.GetComponent<DeformBubbleComponent> ();
					if (dbc == null)
						continue;
					dbc.growTemporary = true;
				}
			} 

			AudioSource audio = GetComponent<AudioSource> ();
			audio.pitch = 1;
			audio.Play ();
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			Material m = gameObject.GetComponent<Renderer> ().material;
			GameObject container = m.GetBubbleColorContainer ();
			if (container != null) {
				Transform[] bubbles = container.GetComponentsInChildren<Transform> ();
				foreach (Transform bubble in bubbles) {
					DeformBubbleComponent dbc = bubble.gameObject.GetComponent<DeformBubbleComponent> ();
					if (dbc == null)
						continue;
					dbc.growTemporary = false;
				}
			}

			AudioSource audio = GetComponent<AudioSource> ();
			audio.pitch = 1;
		}
	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			AudioSource audio = GetComponent<AudioSource> ();
			//audio.pitch *= 0.99F;
			//if (!audio.isPlaying)
			//	audio.Play ();
		}
	}
}
