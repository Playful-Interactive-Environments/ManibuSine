﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CanonManager : NetworkBehaviour {

    [SyncVar]
    public GameObject canonPivot;
    public Transform gunner;

    private Camera mainCamera;
    private Canon canon;

    void Start()
    {
        mainCamera = Camera.main;

        canon = canonPivot.GetComponentInChildren<Canon>();

        //InvokeRepeating("Shoot", 1, 0.1f);
    }


    // PlayerAssigned Msg sent in cannon trigger
    void PlayerAssigned(Transform gunner)
    {
        this.gunner = gunner;
	}

    // PlayerGone Msg sent in cannon trigger
    void PlayerGone ()
    {
        gunner = null;
	}

    void Shoot()
    {
        canon.Shoot();
    }

    void Update()
    {
        if (gunner != null)
        {
            print("update " + gunner.transform.position);
            canonPivot.transform.rotation = mainCamera.transform.rotation;
            canonPivot.transform.position = new Vector3(canonPivot.transform.position.x, canonPivot.transform.position.y, gunner.transform.position.z);
        }
    }
}
