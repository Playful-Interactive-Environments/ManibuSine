using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CanonTrigger : NetworkBehaviour {

    private GameObject assignedPlayer;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NetworkPlayer") {
            assignedPlayer = other.gameObject;
            SendMessageUpwards("PlayerAssigned", assignedPlayer.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "NetworkPlayer") //&& other.GetComponent<NetworkIdentity>().playerControllerId == assignedPlayer.GetComponent<NetworkIdentity>().playerControllerId
        {
            SendMessageUpwards("PlayerGone", other.transform);
        }
    }
}
