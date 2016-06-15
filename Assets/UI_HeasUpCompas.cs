using UnityEngine;
using System.Collections;

public class UI_HeasUpCompas : MonoBehaviour {

    private Transform playerTransform;
    public Transform waypointTransform;
    private MeshRenderer mRenderer;

    public void SetPlayer(Transform player)
    {
        playerTransform = player;
    }

    void Start()
    {
        mRenderer = GetComponentInChildren<MeshRenderer>();
        mRenderer.enabled = false;
        WaypointLevel.NextWaypoint += GetNextWaypoint;
    }

    private void GetNextWaypoint(IEventTrigger waypoint)
    {
        if (waypoint == null)
        {
            mRenderer.enabled = true;
            waypointTransform = null;
        }
        else
        {
            waypointTransform = waypoint.GetTransform();
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (playerTransform == null || waypointTransform == null)
        {
            if (mRenderer.enabled)
                mRenderer.enabled = false;

            return;
        }


        transform.position = playerTransform.transform.position;
        transform.LookAt(waypointTransform);
	}

    void Dispose()
    {
        WaypointLevel.NextWaypoint -= GetNextWaypoint;
    }
}