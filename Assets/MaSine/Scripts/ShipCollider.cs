using UnityEngine;
using System.Collections;

public delegate void ShipDelegate(int damage);
public class ShipCollider : MonoBehaviour {

    public static ShipDelegate ShipHit;

    void OnTriggerEnter(Collider other)
    {
        if (ShipHit != null)
            ShipHit(1);
    }
}