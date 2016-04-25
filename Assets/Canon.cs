﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Canon : MonoBehaviour {

    public GameObject bulletPrefab;

    NetworkPlayer networkPlayer;

    void Start()
    {
        InvokeRepeating("RegisterAtNetworDataManager", 2.5f, 0.5f);
    }

    void RegisterAtNetworDataManager()
    {
        NetworkPlayer nwp = FindObjectOfType<NetworkPlayer>();
        if (nwp.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            networkPlayer = nwp;
            networkPlayer.EventShoot += Shoot;
        }

        if (networkPlayer != null)
            CancelInvoke("RegisterAtNetworDataManager");
    }

    void Shoot()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.up, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().velocity = transform.up * 1000.0f;
        Destroy(bullet, 10.0f);

        print("canon: shoot =)");

        //NetworkServer.Spawn(bullet);
    }
}