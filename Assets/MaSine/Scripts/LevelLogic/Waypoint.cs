﻿using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    private int triggerID = 0;
    [SerializeField]
    private int activateOn = 0;

    void Start()
    {
        EventTrigger.ShipEnteredEvent += ShipEnteredWaypoint;
    }

    public void EnableWaypoint()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
            mr.enabled = true;
    }

    public void DisableWaypoint()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.enabled = false;
        }
        Collider c = GetComponent<Collider>();
        if (c != null) // only use once - therefore deactivate
            c.enabled = false;
    }

    private void ShipEnteredWaypoint(IEventTrigger waypoint)
    {
        if (!(waypoint is MajorEventTrigger))
            return;

        if (waypoint.GetID() == activateOn)
        {
            EnableWaypoint();
        }
        else if (waypoint.GetID() == triggerID)
        {

            DisableWaypoint();
        }
    }

    public int TriggerID
    {
        get { return triggerID; }
        set
        {
            triggerID = value;
            activateOn = triggerID - 1;
        }
    }

    void OnDestroy()
    {
        EventTrigger.ShipEnteredEvent -= ShipEnteredWaypoint;
    }
}