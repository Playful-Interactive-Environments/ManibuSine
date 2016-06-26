using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Canon : NetworkBehaviour {

    public GameObject bulletPrefab;

    NetworkPlayer networkPlayer;

    public void Shoot()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + Vector3.forward, transform.rotation);
        NetworkServer.Spawn(bullet);
    }

    
}