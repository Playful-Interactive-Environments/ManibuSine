using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointLevel : MonoBehaviour {
    [SerializeField]
    public Waypoint[] waypoints;
	// Use this for initialization
	void Start () {
        waypoints = GetComponentsInChildren<Waypoint>();

        Waypoint.ShipEnteredEvent += ShipEnteredWaypoint;
        Waypoint.ShipLeftEvent += ShipLeftWaypoint;
	}

    void ShipEnteredWaypoint(IWaypoint waypoint)
    {
        print("WP entered " + waypoint.GetID());
    }

    private void ShipLeftWaypoint(IWaypoint waypoint)
    {
        print("WP exit " + waypoint.GetID());
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}