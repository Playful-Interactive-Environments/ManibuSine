using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Particle = UnityEngine.ParticleSystem.Particle;

public static class ParticleSystemExtensions
{
	/// <summary>
	/// Method that takes a Particle argument
	/// </summary>
	public delegate Particle UpdateParticleDelegate (Particle particle);
	
	/// <summary>
	/// Get particles Array
	/// </summary>
	public static Particle[] GetParticles (this ParticleSystem particleSystem)
	{
		var particles = new Particle[particleSystem.particleCount];
		particleSystem.GetParticles (particles);                
		return particles;
	}
	
	/// <summary>
	/// Set particles Array
	/// </summary>
	public static void SetParticles (this ParticleSystem particleSystem, Particle[] particles)
	{
		particleSystem.SetParticles (particles, particles.Count ());
	}    
	
	/// <summary>
	/// Applies the given function to each particle in a ParticleSystem
	/// </summary>
	/// <param name="func">Function to perform on each particle</param>
	public static void UpdateParticles (this ParticleSystem particleSystem, UpdateParticleDelegate func)
	{
		var particles = new Particle[particleSystem.particleCount];
		var particleCount = particleSystem.GetParticles (particles);        
		int i = 0;

		while (i < particleCount) {
			particles [i] = func.Invoke (particles [i]);
			i++;
		}
		particleSystem.SetParticles (particles, particleCount);
	}  
	
	public static void InitParticles (this ParticleSystem particleSystem, UpdateParticleDelegate func)
	{
		var particles = new Particle[particleSystem.particleCount];
		var particleCount = particleSystem.GetParticles (particles);        
		int i = 0;
		while (i < particleCount) {
			if (particles[i].lifetime == particles[i].startLifetime)
				particles [i] = func.Invoke (particles [i]);
			i++;
		}
		particleSystem.SetParticles (particles, particleCount);
	}  
	
	public static Vector3 GetParticleDirection (Particle p) {
		Vector3 v = p.velocity.normalized;
		return v;
	}
}    