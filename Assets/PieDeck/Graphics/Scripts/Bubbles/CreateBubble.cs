using UnityEngine;
using System.Collections;

public class CreateBubble : MonoBehaviour {

	public GameObject bubbleObj;
	public GameObject bubbleContainer;

	private int bubbleRandRange = 1000; // 600;

	void FixedUpdate () {
		if ((int)Random.Range (0, bubbleRandRange) == 1) {
			Vector3 scale = new Vector3 ();
			float size = Random.Range (0.1F, 1);
			scale.x = size;
			scale.y = size;
			scale.z = size;

			Vector3 pos = transform.position;

			GameObject b = Instantiate (bubbleObj);
			b.transform.position = pos;
			b.transform.localScale = scale;
			if (bubbleContainer == null)
				b.transform.parent = bubbleObj.transform.parent;
			else
				b.transform.parent = bubbleContainer.transform;
			b.SetActive (true);
			Rigidbody rb = b.GetComponent<Rigidbody> ();
			rb.AddForce (Vector3.up * Random.Range (100, 200));
		}

	}
}
