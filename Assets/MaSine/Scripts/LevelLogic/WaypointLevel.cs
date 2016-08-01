using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WaypointLevel : MonoBehaviour {
    public static event ShipInteracted NextWaypoint;
    [SerializeField]
    public EventTrigger[] eventTriggers;

    private float gameStartsInXSeconds = 2f;


	// Use this for initialization
	void Start () {
        ShipManager.GameOver += OnGameOver;
        eventTriggers = GetComponentsInChildren<EventTrigger>();

        EventTrigger.ShipEnteredEvent += ShipEnteredWaypoint;
        EventTrigger.ShipLeftEvent += ShipLeftWaypoint;

        Invoke("StartGame", gameStartsInXSeconds);
	}

    private void OnGameOver(int damage)
    {
        this.enabled = false;
    }

    void StartGame()
    {
        int id = 1;
        foreach (EventTrigger item in eventTriggers) {
            item.SetID(id++);
        }

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
        StartCoroutine(SyncDelayed(currentLevelState));
    }

    IEnumerator SyncDelayed(int currentLevelState)
    {
        yield return 0;
        for (int i = 0; i <= currentLevelState; i++)
        {
            NextWaypoint(eventTriggers[i]);
        }
    }

}