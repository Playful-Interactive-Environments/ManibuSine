using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CanonTrigger : NetworkBehaviour {
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NetworkPlayer") {
            SendMessageUpwards("PlayerAssigned", other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "NetworkPlayer")
        {
            SendMessageUpwards("PlayerGone", other.transform);
        }
    }
}
