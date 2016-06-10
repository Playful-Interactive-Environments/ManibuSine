using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {
    private int triggerID = 0;
    public int TriggerID
    {
        get { return triggerID; }
        set { triggerID = value; }
    }
}
