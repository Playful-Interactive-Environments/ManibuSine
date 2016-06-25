using UnityEngine;
using System.Collections;

public class CannonPivot : MonoBehaviour {
    [Range(-89, 89)]
    public float horizontalLimit;
    [Range(-89, 89)]
    public float verticalLimit;
    public static CanonDelegateID OutOfRange;
    public static CanonDelegateID InRange;
    bool isRotationLimitHit = false;
    bool wasSentOut = false;
    bool wasSentIn = false;
    CanonManager canonManager;
    // Use this for initialization
    void Start () {
        canonManager = GetComponentInParent<CanonManager>();
	}
	
	// Update is called once per frame
	void Update () {
        LimitRotation();

        if (isRotationLimitHit && !wasSentOut)
        {
            if (OutOfRange != null) OutOfRange(canonManager.netId.Value);
            wasSentOut = true;
        }
        if (!isRotationLimitHit && !wasSentIn)
        {
            if(InRange != null) InRange(canonManager.netId.Value);
            wasSentIn = true;
        }

    }

    private void LimitRotation()
    {
        isRotationLimitHit = false;
        float rotX = transform.localRotation.eulerAngles.x;
        float rotZ = transform.localRotation.eulerAngles.z;
        if (transform.localRotation.eulerAngles.y > 270.0f + horizontalLimit)
        {
            transform.localRotation = Quaternion.Euler(rotX, 270.0f + horizontalLimit, rotZ);
            isRotationLimitHit = true;
        }
        if (transform.localRotation.eulerAngles.y < 90.0f - horizontalLimit)
        {
            transform.localRotation = Quaternion.Euler(rotX, 90.0f - horizontalLimit, rotZ);
            isRotationLimitHit = true;
        }
        float rotY = transform.localRotation.eulerAngles.y;
        if (transform.localRotation.eulerAngles.x < 270.0f - verticalLimit && transform.localRotation.eulerAngles.x >= 180.0f)
        {
            transform.localRotation = Quaternion.Euler(270.0f - verticalLimit, rotY, rotZ);
            isRotationLimitHit = true;
        }
        if (transform.localRotation.eulerAngles.x > 90.0f + verticalLimit && transform.localRotation.eulerAngles.x < 180.0f)
        {
            transform.localRotation = Quaternion.Euler(90.0f + verticalLimit, rotY, rotZ);
            isRotationLimitHit = true;
        }
        if (!isRotationLimitHit) wasSentOut = false;
        if (isRotationLimitHit) wasSentIn = false;
    }
}
