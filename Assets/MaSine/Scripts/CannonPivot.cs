using UnityEngine;
using System.Collections;

public class CannonPivot : MonoBehaviour {

    public float horizontalLimit;
    public float verticalLimit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
        LimitRotation();
    }

    private void LimitRotation()
    {
        float rotX = transform.rotation.x;
        float rotZ = transform.rotation.z;
        if (transform.rotation.y > 0 + horizontalLimit) transform.rotation = Quaternion.Euler(rotX, 0.0f + horizontalLimit, rotZ);
        if (transform.rotation.y < -180 - horizontalLimit) transform.rotation = Quaternion.Euler(rotX, -180.0f - horizontalLimit, rotZ);
        float rotY = transform.rotation.y;
        if (transform.rotation.x > 90 + verticalLimit) transform.rotation = Quaternion.Euler(90.0f + verticalLimit, rotY, rotZ);
        if (transform.rotation.x < -90 - verticalLimit) transform.rotation = Quaternion.Euler(-90.0f - verticalLimit, rotY, rotZ);
    }
}
