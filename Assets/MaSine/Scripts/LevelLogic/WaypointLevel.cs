﻿using UnityEngine;
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
            print("major");
        }
    }

    private void ShipLeftWaypoint(IEventTrigger waypoint)
    {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}