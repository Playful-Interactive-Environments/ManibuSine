using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Canon : NetworkBehaviour {

    public GameObject bulletPrefab;

    NetworkPlayer networkPlayer;

    //void Start()
    //{
    //    InvokeRepeating("RegisterAtNetworDataManager", 2.5f, 0.5f);
    //}

    //void RegisterAtNetworDataManager()
    //{
    //    NetworkPlayer nwp = FindObjectOfType<NetworkPlayer>();
    //    if (nwp.GetComponent<NetworkIdentity>().isLocalPlayer)
    //    {
    //        networkPlayer = nwp;
    //        networkPlayer.EventShoot += Shoot;
    //    }

    //    if (networkPlayer != null)
    //        CancelInvoke("RegisterAtNetworDataManager");
    //}

    public void Shoot()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position , transform.rotation);
        NetworkServer.Spawn(bullet);
    }

    
}