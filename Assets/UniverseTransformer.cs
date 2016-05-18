using UnityEngine;
using System.Collections;

public class UniverseTransformer : MonoBehaviour {

    private static UniverseTransformer instance;
    public static UniverseTransformer Instance { get { return instance; } }

    private Transform targetTransfrom;
    public Transform shipTransform;

    private Rigidbody targetBody;

    private float lerpSpeed = 10;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    public void SetTargetTransform(Transform target) {
        targetTransfrom = target;
        targetBody = target.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (targetTransfrom == null)
            return;

        transform.position = Vector3.Lerp(transform.position, targetTransfrom.position, lerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTransfrom.rotation, lerpSpeed * Time.deltaTime);

    }

    public void MoveForward(float s)
    {
        targetTransfrom.Translate(shipTransform.right * -s * Time.deltaTime, Space.World);
        //targetBody.AddForce(0, 0, s);
    }

    public void RotateUniverse(float a)
    {
        targetTransfrom.RotateAround(shipTransform.position, Vector3.up, -a * Time.deltaTime);
        //targetBody.AddTorque(0, a, 0);
    }
}
