using UnityEngine;
using System.Collections;

public class PickBubble : MonoBehaviour {

	public GameObject player;
	public GameObject blobAnimation;

	private int age = 0;
	private int maxAge = 500;
	private bool picked = false;
	private Material defaultMaterial;

	private System.Collections.Generic.List<GameObject> laserList;

	void Start() {
		GameObject[] laser = GameObject.FindGameObjectsWithTag ("Laser");

		laserList = new System.Collections.Generic.List<GameObject> (laser);
		defaultMaterial = GetComponent<Renderer> ().material;
	}

	void FixedUpdate() {
		if (!picked) {
			age++;

			if (age >= maxAge)
				Destroy (gameObject);
		}
	}

	void OnTriggerEnter (Collider other) {

		if (picked)
			return;

		if (other.gameObject == player) {
			StartCoroutine ( showDestroyAnimation() );
			Destroy (gameObject);
		}
			
		if (laserList != null && laserList.Contains (other.gameObject)) {
			picked = true;

			Material m = other.gameObject.GetComponent<Renderer> ().material;
			gameObject.GetComponent<Renderer> ().material = m;

			GameObject container = m.GetBubbleColorContainer ();
			if (container != null)
				gameObject.transform.parent = container.transform;

			DeformBubbleComponent b = GetComponent<DeformBubbleComponent> ();
			if (b != null) {
				b.IsRising = false;
			}

			gameObject.tag = "TransformableObject";
		}
	}

	IEnumerator showDestroyAnimation() {
		GameObject b = Instantiate (blobAnimation);
		ParticleSystem ps = b.GetComponent<ParticleSystem> ();
		b.transform.position = transform.position;
		b.transform.parent = transform.parent;
		b.SetActive (true);
		yield return new WaitForSeconds (1);
		b.SetActive (false);
		Destroy(b);
	}
}