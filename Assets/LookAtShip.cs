using UnityEngine;
using System.Collections;

public class LookAtShip : MonoBehaviour {
    private Transform shipTransform;
    // Use this for initialization
    void Start () {
        shipTransform = ShipManager.Instance.transform;
	}

    void Update() {
        if (shipTransform == null)
            return;
        transform.LookAt(shipTransform);
    }
}
