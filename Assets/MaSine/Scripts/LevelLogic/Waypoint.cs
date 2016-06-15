using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour
{
    private int triggerID = 0;
    private int activateOn = 0;

    void Start()
    {
        EventTrigger.ShipEnteredEvent += ShipEnteredWaypoint;
    }

    private void ShipEnteredWaypoint(IEventTrigger waypoint)
    {
        if (waypoint.GetID() == activateOn)
        {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            if (mr != null)
                mr.enabled = true;
        }
        else if (waypoint.GetID() == triggerID)
        {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            if (mr != null)
                mr.enabled = false;
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

    void Dispose()
    {
        EventTrigger.ShipEnteredEvent -= ShipEnteredWaypoint;
    }
}