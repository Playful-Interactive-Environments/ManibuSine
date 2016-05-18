﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class SteeringStation : NetworkBehaviour {

    public delegate void SteeringDelegateTransform(SteeringStation steeringStation);
    public SteeringDelegateTransform  EnteredSteering, ExitedSteering;

    public float speedInput;
    public float angleInput;

    public Transform navigator;
    public GameObject assignedPlayer;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(navigator != null)
        {
            calculateSpeedInput();
            calculateAngleInput();
        }
	
	}

    public void SetAssignedPlayer(GameObject player)
    {
        assignedPlayer = player;
    }

    private void calculateAngleInput()
    {
        throw new NotImplementedException();
    }

    private void calculateSpeedInput()
    {
        SteeringTrigger steeringTrigger = GetComponentInChildren<SteeringTrigger>();
        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(navigator.position.x, navigator.position.z));
        if (distance >= 0.00001)
        {
            speedInput = 0.0f;
            return;
        }
            
        speedInput = (this.transform.lossyScale.x / 2 - steeringTrigger.transform.lossyScale.x / 2) / (distance - steeringTrigger.transform.lossyScale.x / 2); 
    }

    // PlayerAssigned Msg sent in cannon trigger
    void PlayerAssigned(Transform navigator)
    {
        this.navigator = navigator;

        if (EnteredSteering != null)
            EnteredSteering(this);
    }

    // PlayerGone Msg sent in cannon trigger
    void PlayerGone()
    {
        navigator = null;

        if (ExitedSteering != null)
            ExitedSteering(this);
    }

    void OnTriggerExit(Collider other)
    {
        if (assignedPlayer !=null && other.tag == "NetworkPlayer" && other.GetComponent<NetworkIdentity>().playerControllerId == assignedPlayer.GetComponent<NetworkIdentity>().playerControllerId)
        {
            PlayerGone();
        }
    }
}
