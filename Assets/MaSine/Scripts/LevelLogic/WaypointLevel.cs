using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointLevel : MonoBehaviour {
    public static event ShipInteracted NextWaypoint;
    [SerializeField]
    public EventTrigger[] eventTriggers;

    private float gameStartsInXSeconds = 1;


	// Use this for initialization
	void Start () {
        eventTriggers = GetComponentsInChildren<EventTrigger>();

        int id = 0;
        foreach (EventTrigger item in eventTriggers)
        {
            item.SetID(id++);
        }

        EventTrigger.ShipEnteredEvent += ShipEnteredWaypoint;
        EventTrigger.ShipLeftEvent += ShipLeftWaypoint;

        Invoke("StartGame", gameStartsInXSeconds);
	}

    void StartGame()
    {
        if (eventTriggers[0] is MajorEventTrigger)
        {
            if (NextWaypoint != null)
                NextWaypoint(eventTriggers[0]);
        }
    }


    void ShipEnteredWaypoint(IEventTrigger waypoint)
    {
        if (waypoint is MajorEventTrigger)
        {
            if (waypoint.GetID() < eventTriggers.Length)
            {
                if (NextWaypoint != null)
                    NextWaypoint(eventTriggers[waypoint.GetID()]);
            }
            else
            {
                if (NextWaypoint != null)
                    NextWaypoint(null);
            }
        }
    }

    private void ShipLeftWaypoint(IEventTrigger waypoint)
    {

    }
	
	void Dispose () {
        EventTrigger.ShipEnteredEvent -= ShipEnteredWaypoint;
        EventTrigger.ShipLeftEvent -= ShipLeftWaypoint;
	}

    public void SyncLevelProgress(int currentLevelState)
    {
        for (int i = 0; i < currentLevelState; i++)
        {
            NextWaypoint(eventTriggers[i]);
        }
    }
}