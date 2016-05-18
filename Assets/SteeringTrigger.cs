using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SteeringTrigger : NetworkBehaviour
{

    public GameObject assignedPlayer;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NetworkPlayer")
        {
            assignedPlayer = other.gameObject;
            SendMessageUpwards("PlayerAssigned", assignedPlayer.transform);
            SendMessageUpwards("SetAssignedPlayer", assignedPlayer);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (assignedPlayer != null && other.tag == "NetworkPlayer" && other.GetComponent<NetworkIdentity>().playerControllerId == assignedPlayer.GetComponent<NetworkIdentity>().playerControllerId)
        {
            assignedPlayer = null;
        }
    }
}
