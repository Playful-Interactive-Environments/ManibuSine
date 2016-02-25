using UnityEngine;
using System.Collections;

public class BubbleSpotComponent : MonoBehaviour {

	public GameObject bubble;
	private int bubbleRandRange = 100;

	void FixedUpdate () {
		if ((int)Random.Range (0, bubbleRandRange) == 1) {
			Vector3 scale = new Vector3 ();
			float size = Random.Range (20, 80); //Random.Range (0.1F, 1);
			scale.x = size;
			scale.y = size;
			scale.z = size;

			Vector3 pos = transform.position + new Vector3 (Random.Range (-transform.localScale.x / 2, transform.localScale.x / 2), 0, Random.Range (-transform.localScale.x / 2, transform.localScale.x / 2));

			GameObject b = Instantiate (bubble);
			b.GetComponent<DeformBubbleComponent> ().useWind = false;
			b.transform.position = pos;
			b.transform.localScale = scale;
			b.transform.parent = bubble.transform.parent;
			b.SetActive (true);
			Rigidbody rb = b.GetComponent<Rigidbody> ();
			rb.AddForce (Vector3.up * Random.Range (1000, 3000));
		}

	}
}
