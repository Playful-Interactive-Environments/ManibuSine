using UnityEngine;
using System.Collections;

public class MajorEventTrigger : EventTrigger {
    
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