using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Canon : NetworkBehaviour {

    public GameObject bulletPrefab;

    NetworkPlayer networkPlayer;

    public void Shoot()
    {
        // adding offset to spawn pos (Vector3.forward * 3)
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + Vector3.forward * 3, transform.rotation);
        NetworkServer.Spawn(bullet);
    }

    
}