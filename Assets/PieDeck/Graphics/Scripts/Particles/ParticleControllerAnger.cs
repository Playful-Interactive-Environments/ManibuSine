using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleControllerAnger : MonoBehaviour {

	// Public Variables
	public float speed = 3;
	public float distance = 2;
	public int interval = 20;
	
	// Private Variables
	private float randOffset;
	private float startTime;
	private float variationSpeed;

	ParticleSystem m_system;
	Dictionary <Vector3, Vector3> directionDictionary;

	void Start () {
		m_system = GetComponent<ParticleSystem> ();
		directionDictionary = new Dictionary<Vector3, Vector3> ();
		startTime = -1;
		randOffset = Random.Range(0, 100);
		variationSpeed = speed;
	}
	
	void Update () {
		int frameNo = Time.frameCount;

		if (startTime == -1) startTime = Time.time;
		float timeFrame = Time.time - startTime;

		if (timeFrame > (2 * Mathf.PI) / variationSpeed) {
			variationSpeed = Random.Range(0.5F, 2 * speed);
			startTime = Time.time;
			timeFrame = 0;
		}

		float radGesInterval = (timeFrame * variationSpeed) % (Mathf.PI * 2);
		float radDeltaInterval = (2 * Mathf.PI) * (interval / 2) / (interval - 1);

		float radInterval1 = radGesInterval * (interval / 2) / (interval - 1); 
		float radInterval2 = radGesInterval * (interval / 2); 

		float sin1 = Mathf.Sin(radInterval1 - radDeltaInterval);
		float sin2 = Mathf.Sin(radInterval2 * 3);

		int sign = -1;		
		float weightDistance = (distance * variationSpeed);
		if ( (int)(radGesInterval / ((Mathf.PI * 2) / interval)) == 0 ) { 
				sign = 1;
			weightDistance *= (interval - 1);
			weightDistance *= 3;
		}

		float factor = sin1;
		if (sign > 0) {
			factor = sin2;

			float intervalLength = (Mathf.PI * 2) / interval;
			float plusInterval = radGesInterval % intervalLength;
			float subIntervalLenght = intervalLength / 3;
			float sector = (int)(plusInterval / subIntervalLenght);

			if ((int)(plusInterval / (intervalLength / 3)) != 1) 
				factor = 0.001F;
		}

		factor = Mathf.Abs (factor) * sign;

		ParticleSystem.Particle[] p = new ParticleSystem.Particle[m_system.particleCount];
		int pCount = m_system.GetParticles(p);

		for (int i = 0; i < p.Length; i++) {
			ParticleSystem.Particle p0 = p[i];

			Vector3 dirVector = p0.velocity.normalized;
			dirVector.x = Mathf.Round( dirVector.x * 1000 ) / 1000;
			dirVector.y = Mathf.Round( dirVector.y * 1000 ) / 1000;
			dirVector.z = Mathf.Round( dirVector.z * 1000 ) / 1000;

			if (!directionDictionary.ContainsKey(dirVector)) {
				directionDictionary.Add(dirVector, dirVector);
				if (dirVector != (-1) * dirVector)
					directionDictionary.Add((-1) * dirVector, dirVector);
			}

			dirVector = directionDictionary[dirVector]; 
			
			p0.velocity = dirVector * factor * weightDistance; 
			
			p0.size *= (Mathf.PerlinNoise(Time.time, p0.size * randOffset)/5 + 0.90F);
			
			
			float noiseA = Mathf.PerlinNoise(Time.time, p0.size * randOffset) - 0.5F;
			float noiseB = Mathf.PerlinNoise(p0.size * randOffset, Time.time) - 0.5F;
			float noiseC = Mathf.PerlinNoise(Time.time * randOffset, p0.size * randOffset) - 0.5F;

			noiseA *= (distance / 2);
			noiseB *= (distance / 2);
			noiseC *= (distance / 2);

			float posDamping = 1;
			p0.position += new Vector3(noiseA/posDamping, noiseB/posDamping, noiseC/posDamping);

			p[i] = p0;
		}
		
		
		m_system.SetParticles ( p, pCount );

	}
}
