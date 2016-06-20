using UnityEngine;
using System.Collections;
public delegate void ShipInteractedID(IEventTrigger trigger, int[] spawnerId);

public class AsteroidEventTrigger : EventTrigger {
    public static event ShipInteractedID ShipEnteredEventID;
    public static event ShipInteractedID ShipLeftEventID;
    [SerializeField]
    public int[] spawnerId;
    public override void ShipEntered()
    {
        base.ShipEntered();
        if (ShipEnteredEventID != null)
            ShipEnteredEventID(this, spawnerId);
    }
    public override void ShipLeft()
    {
        if (ShipLeftEventID != null)
            ShipLeftEventID(this, spawnerId);
    }
}