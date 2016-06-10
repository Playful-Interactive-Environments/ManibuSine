using UnityEngine;
using System.Collections;

public class EventTrigger : MonoBehaviour, IEventTrigger
{
    public static event ShipInteracted ShipEnteredEvent;
    public static event ShipInteracted ShipLeftEvent;
    [SerializeField]
    private int id;

    public int GetID()
    {
        return id;
    }
    public virtual void SetID(int id)
    {
        this.id = id;
    }

    public void ShipEntered()
    {
        if (ShipEnteredEvent != null)
            ShipEnteredEvent(this);
    }
    public void ShipLeft()
    {
        if (ShipLeftEvent != null)
            ShipLeftEvent(this);
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO check if tag = ship (ship has no tag yet)
        ShipEntered();
    }
    void OnTriggerExit(Collider other)
    {
        // TODO check if tag = ship (ship has no tag yet)
        ShipLeft();
    }
}