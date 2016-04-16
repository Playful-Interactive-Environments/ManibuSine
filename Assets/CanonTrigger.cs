using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CanonTrigger : NetworkBehaviour {

    [SyncVar]
    public GameObject assignedPlayer;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player" && assignedPlayer == null) {
            assignedPlayer = other.gameObject;
        }
    }

    void OnTriggerExti(Collider other)
    {
        assignedPlayer = null;
    }

	void Start () {
	
	}

	void Update () {
	
	}
}
