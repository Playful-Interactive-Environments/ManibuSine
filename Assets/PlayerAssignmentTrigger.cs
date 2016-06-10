using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerAssignmentTrigger : NetworkBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NetworkPlayer")
        {
            SendMessageUpwards("MsgPlayerAssigned", other.transform, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "NetworkPlayer")
        {
            SendMessageUpwards("MsgPlayerGone", other.transform, SendMessageOptions.DontRequireReceiver);
        }
    }
}
