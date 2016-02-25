using UnityEngine;
using System.Collections;

public class DestroyOldBubbleBlobs : MonoBehaviour {
	
	void FixedUpdate () {
		ParticleSystem[] psList = GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem ps in psList) {
			if (ps.gameObject.activeSelf && !ps.loop && ps.particleCount == 0)
				Destroy (ps.gameObject);
		}
	}
}
