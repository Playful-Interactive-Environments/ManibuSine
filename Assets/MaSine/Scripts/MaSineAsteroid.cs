﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MaSineAsteroid : NetworkBehaviour {

    NetworkPlayer player;
    private float speed = 10000.0f;

	// Use this for initialization
	void Start () {
        if (isServer)
            GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * speed);
	}

    void OnTriggerEnter(Collider other)
    {
        //player.CmdDestroyEntity(gameObject);
        if (isServer)
        {
            Destroy(gameObject);
        }  
    }

	// Update is called once per frame
	void Update () {
	
	}
}