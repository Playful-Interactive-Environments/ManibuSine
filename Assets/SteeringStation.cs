using UnityEngine;
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

    public UniverseTransformer universeTransformer;

    private float speedMulti = 1000;
    private float angleMulti = 0.2f;

    private NetworkPlayer networkPlayer;
    private 

    // Use this for initialization
    void Start () {
        InvokeRepeating("RegisterAtNetworDataManager", 0.5f, 0.5f);
    }

    void RegisterAtNetworDataManager()
    {
        NetworkPlayer nwp = FindObjectOfType<NetworkPlayer>();
        if (nwp != null && nwp.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            networkPlayer = nwp;
        }

        if (networkPlayer != null)
            CancelInvoke("RegisterAtNetworDataManager");
    }

    // Update is called once per frame
    void Update () {
        if (isServer)
            return;
        if(navigator != null)
        {
            CalculateSpeedInput();
            CalculateAngleInput();
            if (angleInput > 90 || angleInput < -90)
                return; 
                

            networkPlayer.CmdMoveShipForward(speedInput * speedMulti);
            networkPlayer.CmdRotateShipCW(angleInput * angleMulti);
        }
	
	}

    public void SetAssignedPlayer(GameObject player)
    {
        assignedPlayer = player;
    }

    private void CalculateAngleInput()
    {
        float x = assignedPlayer.transform.position.x - transform.position.x;
        float y = assignedPlayer.transform.position.z - transform.position.z;

        angleInput = Mathf.Rad2Deg * Mathf.Atan2(y, x);
        Debug.DrawRay(this.transform.position, 
            new Vector3(Mathf.Cos(Mathf.Atan2(y, x)), 
            0,  
            Mathf.Sin(Mathf.Atan2(y, x))) * 10000, Color.blue);
    }

    private void CalculateSpeedInput()
    {
        SteeringTrigger steeringTrigger = GetComponentInChildren<SteeringTrigger>();
        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(navigator.position.x, navigator.position.z));

        if (distance < steeringTrigger.transform.lossyScale.x / 2) 
        {
            speedInput = 0.0f;
            return;
        }
            
        speedInput = (distance - steeringTrigger.transform.lossyScale.x / 2) / (this.transform.lossyScale.x / 2 - steeringTrigger.transform.lossyScale.x / 2); 
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
