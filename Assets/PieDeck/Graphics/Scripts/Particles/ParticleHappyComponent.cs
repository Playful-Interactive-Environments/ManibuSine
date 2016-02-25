using UnityEngine;
using System.Collections;

public class ParticleHappyComponent : MonoBehaviour {

	private int velocityInterval = 20;
	private float jumpingHeight = 15;

	private float speedJump = 10;
	private float speedTwisting = 1;// 2;
	private float distanceJump = 0.3F;
	private float distanceTwisting = 0.05F;
	private float rotationJump = 10;
	private float rotationTwisting = 90;

	private float rotationOffset = 0;

	private float deltaAngle = -57;

	private float offsetX, offsetY;

	ParticleSystem m_system;

	void Start () {
		m_system = GetComponent<ParticleSystem> ();
		ParticleSystem.VelocityOverLifetimeModule vel = m_system.velocityOverLifetime;

		AnimationCurve c = new AnimationCurve ();

		for (int i = 0; i <= velocityInterval; i++) {
			float height = (i+1) % 2;
			height = height / 2F + 0.5F;
			height -= ((velocityInterval - i) / (float)velocityInterval) * 0.9F;
			c.AddKey (i / (float)velocityInterval, height);
		}

		vel.y = new ParticleSystem.MinMaxCurve (jumpingHeight, c);

		offsetX = Random.Range(0, 100);
		offsetY = Random.Range(0, 100);
		transform.position += new Vector3 (0, 0, -2);
	}
	
	void FixedUpdate () {
		float x = Mathf.PerlinNoise (offsetX + Time.time, 0.0F) * 2;
		float y = Mathf.PerlinNoise (0.0F, offsetY + Time.time) * 2;
		float z = Mathf.PerlinNoise (offsetX + Time.time, offsetY + Time.time);

		Vector3 posDelta = new Vector3 ();
		posDelta.x = Mathf.Cos (Time.time * speedTwisting) * (distanceTwisting * x );
		posDelta.y = Mathf.Sin (Time.time * speedJump) * (distanceJump * y);
		posDelta.z = Mathf.Sin (Time.time * speedTwisting) * (distanceTwisting * x);

		Vector3 rotDelta = new Vector3 ();
		rotDelta.y = Time.deltaTime * deltaAngle;

		transform.Rotate (rotDelta);
		transform.position += posDelta;
	}
}
