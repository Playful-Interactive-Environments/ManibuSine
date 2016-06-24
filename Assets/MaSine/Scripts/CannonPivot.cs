using UnityEngine;
using System.Collections;

public class CannonPivot : MonoBehaviour {
    [Range(-89, 89)]
    public float horizontalLimit;
    [Range(-89, 89)]
    public float verticalLimit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        LimitRotation();
    }

    public void SetRotation(Quaternion rotation)
    {
        float rotX = rotation.eulerAngles.x;
        float rotZ = rotation.eulerAngles.z;
        if (rotation.eulerAngles.y > 270.0f + horizontalLimit) rotation = Quaternion.Euler(rotX, 270.0f + horizontalLimit, 0);
        if (rotation.eulerAngles.y < 90.0f - horizontalLimit) rotation = Quaternion.Euler(rotX, 90.0f - horizontalLimit, 0);
        float rotY = rotation.y;
        if (rotation.eulerAngles.x > 270.0f + verticalLimit) rotation = Quaternion.Euler(270.0f + verticalLimit, rotY, 0);
        if (rotation.eulerAngles.x < 90.0f - verticalLimit) rotation = Quaternion.Euler(90.0f - verticalLimit, rotY, 0);
        transform.rotation = rotation;
        //LimitRotation();
    }

    private void LimitRotation()
    {
        float rotX = transform.localRotation.eulerAngles.x;
        float rotZ = transform.localRotation.eulerAngles.z;
        if (transform.localRotation.eulerAngles.y > 270.0f + horizontalLimit) transform.localRotation = Quaternion.Euler(rotX, 270.0f + horizontalLimit, rotZ);
        if (transform.localRotation.eulerAngles.y < 90.0f - horizontalLimit) transform.localRotation = Quaternion.Euler(rotX, 90.0f - horizontalLimit, rotZ);
        float rotY = transform.localRotation.eulerAngles.y;
        if (transform.localRotation.eulerAngles.x < 270.0f - verticalLimit && transform.localRotation.eulerAngles.x >= 180.0f) transform.localRotation = Quaternion.Euler(270.0f - verticalLimit, rotY, rotZ);
        if (transform.localRotation.eulerAngles.x > 90.0f + verticalLimit && transform.localRotation.eulerAngles.x < 180.0f) transform.localRotation = Quaternion.Euler(90.0f + verticalLimit, rotY, rotZ);
    }
}
