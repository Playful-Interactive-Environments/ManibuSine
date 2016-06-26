﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

    public float speed = 100;
    public float lifeTime = 10.0f;

    // Use this for initialization
    void Start () {
        transform.parent = UniverseTransformer.Instance.transform;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isServer)
        {
            Destroy(gameObject);
        }
    }
}
