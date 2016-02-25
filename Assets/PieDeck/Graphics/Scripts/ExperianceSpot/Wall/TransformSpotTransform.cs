using UnityEngine;
using System.Collections;

public enum TransformSpotType {
	Position = 0,
	Rotation,
	Scale,
	MoveUp,
	MoveDown,
	Empty
}

public class TransformSpotTransform : MonoBehaviour {

	//public GameObject showPoint;

	public TransformSpotComponent trigger;
	public Material[] TransformSpotTypeMaterial = new Material[5];





	private TransformSpotType type = TransformSpotType.Position;

	private float speed = 0.1F;

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space) && trigger.isActive) {
			type = (TransformSpotType)(((int)type + 1) % ((int)TransformSpotType.Empty + 1));
			trigger.GetComponent<Renderer>().material = TransformSpotTypeMaterial[(int)type];
		}
	}

	void OnTriggerStay (Collider other) {
		


		if (other.gameObject.CompareTag ("TransformableObject")) {
			//GetComponent<Rigidbody> ().isKinematic = true;
			Vector3 direction = trigger.player.GetComponent<PlayerMovement> ().movingDirection;
			Rigidbody rb = other.gameObject.GetComponent<Rigidbody> ();

			Vector3 center = other.transform.TransformPoint (Vector3.zero);
			Vector3 transformSpotPos = transform.position; // transform.parent.position;
			Vector3 colliderPoint = other.bounds.ClosestPoint (transformSpotPos); 
			//showPoint.transform.position = colliderPoint;

			switch (type) {
			case TransformSpotType.Position:

				if (!trigger.isMoving)
					break;
				
				//rb.isKinematic = true;
				other.gameObject.transform.position += direction;
				break;
			case TransformSpotType.Rotation:

				if (!trigger.isMoving)
					break;


				Vector3 rot = direction.normalized; 

				float angle = Vector3.Angle(colliderPoint - center, (colliderPoint + direction) - center);

				float angleDirForward = Vector3.Angle(direction, Vector3.forward);
				float angleDirRight = Vector3.Angle(direction, Vector3.right);

				if (angleDirForward > 135 || angleDirRight < 45)
					angle *= -1;

				rot *= angle;

				other.gameObject.transform.Rotate (rot);


				break;
			case TransformSpotType.Scale:
				if (!trigger.isMoving)
					break;

				Vector3 oldDist = colliderPoint - center;
				Vector3 newDist = (colliderPoint + direction) - center;
				Vector3 scaleAxis = new Vector3 (0, 1, 1);

				if (oldDist.sqrMagnitude < newDist.sqrMagnitude)
					other.gameObject.transform.localScale += scaleAxis * (direction.magnitude) * 100;
				else
					other.gameObject.transform.localScale -= scaleAxis * (direction.magnitude) * 100;

				break;
			case TransformSpotType.MoveUp:
				other.gameObject.transform.position += Vector3.up * speed;
				break;
			case TransformSpotType.MoveDown:
				other.gameObject.transform.position -= Vector3.up * speed;
				break;
			}
		}
	}
}
