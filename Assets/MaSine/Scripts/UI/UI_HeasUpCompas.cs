using UnityEngine;
using System.Collections;
using System;

public class UI_HeasUpCompas : MonoBehaviour {

    private float height = 0.66f;

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
        ShipManager.GameOver += OnGameOver;
        mRenderer = GetComponentInChildren<MeshRenderer>();
        mRenderer.enabled = false;
        ship = FindObjectOfType<ShipManager>();
        WaypointLevel.NextWaypoint += OnGetNextWaypoint;

	    htArrowBounce.Add("y", 5.2f);
	    htArrowBounce.Add("time", 2.7f);
	    htArrowBounce.Add("delay", 0.1f);
	    htArrowBounce.Add("looptype",iTween.LoopType.pingPong);
        htArrowBounce.Add("easetype", iTween.EaseType.easeOutElastic);

        iTween.MoveBy(gameObject, htArrowBounce);
    }

    private void OnGameOver(int success) {
        Destroy(gameObject);
    }

    private void OnGetNextWaypoint(IEventTrigger waypoint)
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
            if (transform.parent.name.Contains("Goal")) {
                height = 13;
            }

            transform.localPosition = Vector3.up * height;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (ship == null)
            return;
        transform.LookAt(ship.transform);
	}

    void OnDestroy()
    {
        WaypointLevel.NextWaypoint -= OnGetNextWaypoint;
        ShipManager.GameOver -= OnGameOver;
    }
}