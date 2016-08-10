using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class PlayerAssignmentTrigger : MonoBehaviour
{
    void Start()
    {
        ShipManager.GameOver += OnGameOver;
    }

    private void OnGameOver(int damage)
    {
        enabled = false;
    }

    void OnDestroy()
    {
        ShipManager.GameOver -= OnGameOver;
    }

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
