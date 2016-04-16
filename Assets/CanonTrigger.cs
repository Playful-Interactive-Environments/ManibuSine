using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CanonTrigger : NetworkBehaviour {

    [SyncVar]
    public GameObject assignedPlayer;

    void OnTriggerEnter(Collider other)
    {
        print("enter");
        if (other.tag == "NetworkPlayer") {
            assignedPlayer = other.gameObject;
            print(other.GetComponent<NetworkIdentity>().playerControllerId + "entered");
        }
    }

    void OnTriggerExti(Collider other)
    {
        if (other.tag == "NetworkPlayer")
            assignedPlayer = null;
    }

	void Start () {
	
	}

	void Update () {
	
	}
}
