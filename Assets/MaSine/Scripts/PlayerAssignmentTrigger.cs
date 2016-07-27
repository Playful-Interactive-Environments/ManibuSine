using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerAssignmentTrigger : MonoBehaviour
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
