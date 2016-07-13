using UnityEngine;
using System.Collections;

public class UI_HeasUpCompas : MonoBehaviour {

    private float height = 1.0f;

    private Transform playerTransform;
    private ShipManager ship;
    private MeshRenderer mRenderer;

    Hashtable htArrowBounce = new Hashtable();

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

	    htArrowBounce.Add("y", 5.2f);
	    htArrowBounce.Add("time", 2.7f);
	    htArrowBounce.Add("delay", 0.1f);
	    htArrowBounce.Add("looptype",iTween.LoopType.pingPong);
        htArrowBounce.Add("easetype", iTween.EaseType.easeOutElastic);

        iTween.MoveBy(gameObject, htArrowBounce);
    }

    private void GetNextWaypoint(IEventTrigger waypoint)
    {
        if (waypoint == null)
        {
            if (mRenderer != null)
                mRenderer.enabled = false;
        }
        else
        {
            if (mRenderer != null)
                mRenderer.enabled = true;

            transform.parent = waypoint.GetTransform();
            transform.localPosition = Vector3.up * height;
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
	}

    void OnDestroy()
    {
        WaypointLevel.NextWaypoint -= GetNextWaypoint;
    }
}