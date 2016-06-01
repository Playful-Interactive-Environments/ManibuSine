using UnityEngine;
using System.Collections;

public class ShipMovement : MonoBehaviour {

    private static ShipMovement instance;
    public static ShipMovement Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void MoveForward(float speed)
    {
        this.transform.Translate(this.transform.forward * speed);
    }

    void RotateRight(float rot)
    {
        this.transform.Rotate(transform.up, rot);
    }
}
