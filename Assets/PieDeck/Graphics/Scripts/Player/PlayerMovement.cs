using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;            // The speed that the player will move at.
	public Vector3 movingDirection;
	
	Vector3 movement;                   // The vector to store the direction of the player's movement.
	Animator anim;                      // Reference to the animator component.
	Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
	int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	float camRayLength = 100f;          // The length of the ray from the camera into the scene.

	private float lastRotation = 0;
	private float minDistance = 0.001F;
	private float minSqrDistance;
	private Vector3 lastPos;

	void Start () {
		lastPos = transform.position;
		minSqrDistance = Mathf.Pow (minDistance, 2);
	}

	void Awake ()
	{
		// Create a layer mask for the floor layer.
		floorMask = LayerMask.GetMask ("Floor");
		
		// Set up references.
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
	}
	
	
	void FixedUpdate ()
	{
		// Store the input axes.
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");
		
		// Move the player around the scene.
		Move (h, v);

		calcRotation();
	}
	
	void Move (float h, float v)
	{
		// Set the movement vector based on the axis input.
		movement.Set (h, 0f, v);
		
		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * speed * Time.deltaTime;
		
		// Move the player to it's current position plus the movement.
		playerRigidbody.MovePosition (transform.position + movement);
	}

	public float computeAngle (float moveHorizontal, float moveVertical) {
		return Mathf.Atan2(moveHorizontal, moveVertical);
	}

	public void calcRotation () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		float r = computeAngle(moveHorizontal, moveVertical);

		Vector3 direction = transform.position - lastPos;
		movingDirection = direction;

		if (direction.sqrMagnitude >= minSqrDistance) {
			transform.rotation = Quaternion.identity;

			float degree = Mathf.Rad2Deg * r;
			transform.Rotate(0,degree,0);
			lastRotation = r;
		}


		lastPos = transform.position;
	}
}