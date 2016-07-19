using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Canon : NetworkBehaviour {
    public GameObject bulletPrefab;
    public Transform barrel;
    private AudioSource source;


    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    /// <summary>
    /// used to play effects
    /// sound, particles, ...
    /// </summary>
    public void ShootEffects()
    {
        source.Play();
    }

    public void Shoot()
    {
        // adding offset to spawn pos (Vector3.forward * 3)
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, barrel.position, transform.rotation);
        NetworkServer.Spawn(bullet);
    }
}