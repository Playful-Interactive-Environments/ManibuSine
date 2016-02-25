using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateWall : MonoBehaviour {

	public GameObject wallContainer;
	public Material defaultWallColor;

	private List<Vector3> points;

	private Vector3 startPoint;
	private Vector3 endPoint;
	private Vector3 height;

	private GameObject wall;

	void Start () {
		points = new List<Vector3> ();
		ResetSettings ();
	}

	void ResetSettings() {
		points.Clear ();
		wall = null;

		startPoint = Vector3.zero;
		endPoint = Vector3.zero;
		height = Vector3.zero;
	}

	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) {
			points.Add (transform.position);
		}

		if (points.Count > 0) {
			startPoint = points [0];

			if (points.Count > 1)
				endPoint = points [1];
			else 
				endPoint = transform.position;
		
			if (points.Count > 2)
				height = points [2];
			else height = transform.position;

			if (wall == null) {
				wall = new GameObject ();
				wall.name = "Pivot";
				wall.transform.parent = wallContainer.transform;

				GameObject wallObj = GameObject.CreatePrimitive (PrimitiveType.Cube);
				wallObj.transform.parent = wall.transform;
				wallObj.transform.position = Vector3.one * 0.5F;
				wallObj.name = "Wall";
				wallObj.GetComponent<Renderer> ().material = defaultWallColor;
				wallObj.GetComponent<BoxCollider> ().isTrigger = true;
				Rigidbody rb = wallObj.AddComponent<Rigidbody> ();
				rb.useGravity = false;
				rb.isKinematic = true;
				wallObj.tag = "TransformableObject";
				wallObj.AddComponent<changeBubbleDirection> ();
			}

			wall.transform.position = startPoint;
			wall.transform.localScale = (Vector3.right * 0.0001F) +
										(Vector3.forward * ((endPoint - startPoint).magnitude)) +
										(Vector3.up * (0.5F + (height - endPoint).magnitude));
			wall.transform.rotation = Quaternion.identity;

			Vector3 rot = Vector3.up;

			float angle = Vector3.Angle(endPoint - startPoint, Vector3.forward);
			float angle2 = Vector3.Angle(endPoint - startPoint, Vector3.right);

			if (angle2 > 90)
				angle *= -1;

			rot *= angle;

			wall.transform.Rotate (rot);
		}

		if (points.Count >= 3) {
			GameObject wallObj = wall.transform.GetChild (0).gameObject;
			wallObj.transform.parent = wall.transform.parent;
			ResetSettings ();
			Destroy (wall);
		}
	}
}
