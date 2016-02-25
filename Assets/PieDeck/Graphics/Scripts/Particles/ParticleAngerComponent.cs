using UnityEngine;
using System.Collections;

public class ParticleAngerComponent : MonoBehaviour {
	
	private float speed = 10;
	private float distance = 1;
	private float lastVelocityDelta = 0;
	
	private int interval = 20;
	
	ParticleSystem m_System;
	ParticleSystem.Particle[] m_Particles;
	int[] p_sign;
	
	private float emissionRate;
	
	void Start () {
		m_System = GetComponent<ParticleSystem> ();
		m_Particles = m_System.GetParticles ();
		emissionRate = m_System.emissionRate;
	}
	
	void Update () {
		int t = (int)(Time.time * 3);
		int s = (t % interval);

		float sin1 = Mathf.Sin (Time.time * speed * 2);
		float sin2 = Mathf.Sin (Time.time * speed);
		float cos1 = Mathf.Cos (Time.time * speed * 2);
		float cos2 = Mathf.Cos (Time.time * speed);
		
		int velocitySign = -1; 
		float velocityDelta = Mathf.Abs (distance); 
		
		if (s == 0) {
			velocitySign = 1;
			velocityDelta = velocityDelta / interval * (interval-1); 
		} else {
			velocityDelta = velocityDelta / interval; 
		}
		
		if (s % 4 == 0) {
			velocitySign = 1;
		}
		
		float backup = velocityDelta * velocitySign;
		
		if (Mathf.Sign (lastVelocityDelta) != velocitySign) {
			velocityDelta = velocityDelta * (-1);
		} else {
		}
		lastVelocityDelta = backup;
		
		if (s > 0) { 
			m_System.emissionRate = 0;
		} else {
			m_System.emissionRate = emissionRate * interval;
		}
		
		// Idees
		// 1) Vor zurück abhängig von Lifespan
		// 2) Particles Live forever in eigenen Array speichern und Direction, ... vorberechnen
		
		m_System.UpdateParticles (particle => {
			
			Vector3 dir = ParticleSystemExtensions.GetParticleDirection(particle);
			dir = dir * lastVelocityDelta; //velocityDelta; 
			
			if (lastVelocityDelta < 0) {
				dir *= 2F; 
			}
			

			particle.velocity += dir / 1; 
			return particle;
		});        
	}
}
