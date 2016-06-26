using UnityEngine;
using System.Collections;

public class MaSineParticle : MonoBehaviour {
    // a simple script to scale the size, speed and lifetime of a particle system

    public float multiplier = 3;
    private float maxLifetime = 0;

    private void Start()
    {
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in systems)
        {
            system.startSize *= multiplier;
            system.startSpeed *= multiplier;
            //system.startLifetime *= Mathf.Lerp(multiplier, 1, 0.5f);
            system.Clear();
            system.Play();

            if (system.duration > maxLifetime)
            {
                maxLifetime = system.duration;
            }
        }
        Destroy(gameObject, maxLifetime);
    }
}
