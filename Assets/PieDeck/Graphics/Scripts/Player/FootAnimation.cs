using UnityEngine;
using System.Collections;

public class FootAnimation : MonoBehaviour {

	public float offset = 0;
	public float speed = 2;
	public float height = 1.5F;

	private Vector3 footPos;

	void Start () {
		footPos = transform.localPosition;
	}

	void FixedUpdate () {
		Vector3 pos = transform.localPosition;
		pos.y = footPos.y + Mathf.Abs (Mathf.Sin (Time.time * speed + (offset * Mathf.PI))) * height;
		transform.localPosition = pos;
	}
}
