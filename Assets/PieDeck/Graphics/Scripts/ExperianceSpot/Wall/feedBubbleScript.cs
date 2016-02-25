using UnityEngine;
using System.Collections;

public class feedBubbleScript : MonoBehaviour {

	public GameObject blobAnimation;
	public GameObject[] walls;

	private int feedCount = 0;
	private int maxFeedCount = 30;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag ("Bubble")) {
			feedCount++;
			Destroy (other.gameObject);
			DeformBubbleComponent dbc = GetComponent<DeformBubbleComponent> ();
			dbc.Scale += Vector3.one * 2;
		}

		if (feedCount > maxFeedCount) {
			StartCoroutine ( showDestroyAnimation() );
			foreach (GameObject o in walls) {
				Destroy (o);
			}

			Destroy (gameObject);
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
