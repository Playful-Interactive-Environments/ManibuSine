using UnityEngine;
using System.Collections;
public delegate void ShipInteracted(IWaypoint waypoint);

public interface IWaypoint {
    int GetID();
    void SetID(int id);
    void ShipEntered();
    void ShipLeft();
}