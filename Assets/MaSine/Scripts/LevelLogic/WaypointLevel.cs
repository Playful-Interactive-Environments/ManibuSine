using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointLevel : MonoBehaviour {
    [SerializeField]
    public EventTrigger[] eventTriggers;
	// Use this for initialization
	void Start () {
        eventTriggers = GetComponentsInChildren<EventTrigger>();

        int id = 1;
        foreach (EventTrigger item in eventTriggers)
        {
            item.SetID(id++);
        }

        EventTrigger.ShipEnteredEvent += ShipEnteredWaypoint;
        EventTrigger.ShipLeftEvent += ShipLeftWaypoint;
	}

    void ShipEnteredWaypoint(IEventTrigger waypoint)
    {
        if (waypoint is MajorEventTrigger)
        {

        }
    }

    private void ShipLeftWaypoint(IEventTrigger waypoint)
    {

    }
	
	void Dispose () {
        EventTrigger.ShipEnteredEvent -= ShipEnteredWaypoint;
        EventTrigger.ShipLeftEvent -= ShipLeftWaypoint;
	}
}