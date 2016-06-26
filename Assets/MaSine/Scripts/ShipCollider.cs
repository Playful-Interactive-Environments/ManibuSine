using UnityEngine;
using System.Collections;

public delegate void ShipDelegate(int damage);
public class ShipCollider : MonoBehaviour {

    public static ShipDelegate ShipHit;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (ShipHit != null)
            ShipHit(1);
    }
}