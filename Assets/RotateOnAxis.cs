using UnityEngine;
using System.Collections;

public class RotateOnAxis : MonoBehaviour {
    public Vector3 axis;
	
	// Update is called once per frame
	void Update () {
        if (axis == Vector3.zero)
            return;
        transform.Rotate(axis.x * Time.deltaTime, axis.y * Time.deltaTime, axis.z * Time.deltaTime);
	}
}
