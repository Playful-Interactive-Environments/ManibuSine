using UnityEngine;
using System.Collections;
public delegate void ShipInteracted(IEventTrigger waypoint);

public interface IEventTrigger {
    int GetID();
    void SetID(int id);
    void ShipEntered();
    void ShipLeft();
    Transform GetTransform();
}