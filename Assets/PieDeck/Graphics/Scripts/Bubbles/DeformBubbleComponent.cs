using UnityEngine;
using System.Collections;

public class DeformBubbleComponent : MonoBehaviour {

	public bool growOverLifetime = false;
	public bool growTemporary = false;
	public bool useWind = true;
	private bool isRising = true;

	private float maxSize = 30;

	private float damping_Rotation = 10;
	private float damping_Scaling = 6;
	private float damping_Position = 30;

	private float scaleFactor = 0.01F;
	private float tempScaleFactor = 0.05F;

	private float offsetX, offsetY;
	private Vector3 baseScale;
	private Vector3 basePosition, baseRotation;
	private Vector3 tempScale;

	public Vector3 Scale {
		get { return tempScale; }
		set {
			baseScale = value;
			tempScale = value; 
		}
	}

	void Start () {
		offsetX = Random.Range(0, 100);
		offsetY = Random.Range(0, 100);
		baseScale = transform.localScale;
		basePosition = transform.position;
		baseRotation = transform.eulerAngles;
		tempScale = baseScale;
	}

	public bool IsRising {
		get { return isRising; }
		set { 
			isRising = value; 
			if (!isRising) {
				growOverLifetime = false;
				basePosition = transform.position;
				Rigidbody rb = GetComponent<Rigidbody> ();
				rb.velocity = Vector3.zero;
			}
		}
	}
	
	void FixedUpdate () {
		if (growOverLifetime && baseScale.x < maxSize) {
			baseScale += Vector3.one * scaleFactor;
			tempScale = baseScale;
		}

		if (growTemporary && tempScale.x < maxSize)
			tempScale += Vector3.one * tempScaleFactor;
		else if (tempScale.x > baseScale.x)
			tempScale -= Vector3.one * tempScaleFactor;

		float x = Mathf.PerlinNoise (offsetX + Time.time, 0.0F);
		float y = Mathf.PerlinNoise (0.0F, offsetY + Time.time);
		float z = Mathf.PerlinNoise (offsetX + Time.time, offsetY + Time.time);

		float s_x = 1 + (x - 0.5F) / damping_Scaling;
		float s_y = 1 + (y - 0.5F) / damping_Scaling;
		float s_z = 1 + (z - 0.5F) / damping_Scaling;
		
		float p_x = (x - 0.5F) / damping_Position;
		float p_y = (y - 0.5F) / damping_Position;
		float p_z = (z - 0.5F) / damping_Position;
		
		transform.localScale =  new Vector3(tempScale.x * s_x, tempScale.y * s_y, tempScale.z * s_z);
		transform.Rotate (new Vector3(x / damping_Rotation, y / damping_Rotation, z / damping_Rotation));

		if (!isRising)
			transform.Translate (new Vector3 (p_x, p_y, p_z));
		else if (useWind) {
			Rigidbody rb = GetComponent<Rigidbody> ();

			float factorX = 1;
			float factorY = 1;

			float windX = Mathf.PerlinNoise (transform.position.x * factorX, transform.position.y * factorY);
			windX = (windX - 0.5F) * 0.5F;
			float windZ = Mathf.PerlinNoise (transform.position.z * factorX, transform.position.y * factorY);
			windZ = (windZ - 0.5F) * 0.5F;

			rb.AddForce (new Vector3 (windX, 0, windZ), ForceMode.VelocityChange);
		}
	}
}
