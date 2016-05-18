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
}
