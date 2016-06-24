using UnityEngine;
using System.Collections;

public class UI_HeasUpCompas : MonoBehaviour {

    private Transform playerTransform;
    private Transform waypointTransform = null;
    private ShipManager ship;
    private MeshRenderer mRenderer;

    public void SetPlayer(Transform player)
    {
        playerTransform = player;
    }

    void Start()
    {
        mRenderer = GetComponentInChildren<MeshRenderer>();
        mRenderer.enabled = false;
        ship = FindObjectOfType<ShipManager>();
        WaypointLevel.NextWaypoint += GetNextWaypoint;
    }

    private void GetNextWaypoint(IEventTrigger waypoint)
    {
        if (waypoint == null)
        {
            mRenderer.enabled = false;
            waypointTransform = null;
        }
        else
        {
            mRenderer.enabled = true;
            waypointTransform = waypoint.GetTransform();
            transform.parent = waypoint.GetTransform();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (ship == null)
            return;


        transform.LookAt(ship.transform);
        //if (playerTransform == null || waypointTransform == null)
        //{
        //    //if (mRenderer.enabled)
        //    //    mRenderer.enabled = false;

        //    return;
        //}


        //transform.position = playerTransform.transform.position;

	}

    void Dispose()
    {
        WaypointLevel.NextWaypoint -= GetNextWaypoint;
    }
}