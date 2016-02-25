using UnityEngine;
using System.Collections;

public class AutoMoveCarComponent : MonoBehaviour {

	public float speed = 5;
	public Vector3 direction = Vector3.forward;
	public float maxDistance = 1000;

	void FixedUpdate () {
		transform.position += direction * speed;

		if (transform.position.sqrMagnitude > maxDistance * maxDistance)
			DestroyImmediate (gameObject);
	}
}
