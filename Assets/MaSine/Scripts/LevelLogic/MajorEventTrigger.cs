using UnityEngine;
using System.Collections;

public class MajorEventTrigger : EventTrigger {
    
    public void Visited() {
        Waypoint wp = transform.GetComponentInChildren<Waypoint>();
        if (wp == null)
            return;
        wp.DisableWaypoint();

        // also disable trigger - not needed anymore

        Collider coll = GetComponent<Collider>();
        if (coll != null)
            coll.enabled = false;
    }

    public override void SetID(int id)
    {
        base.SetID(id);
        // activate waypoint logic
        Waypoint wp = transform.GetComponentInChildren<Waypoint>();
        if (wp == null)
        {
            Debug.LogError("No Waypoint found.");
            return;
        }
        wp.gameObject.SetActive(true);
        wp.TriggerID = id;
    }
}