using UnityEngine;
using System.Collections;

public class UniverseTransformer : MonoBehaviour {

    private static UniverseTransformer instance;
    public static UniverseTransformer Instance { get { return instance; } }

    public Transform targetTransfrom;
    public Transform shipTransform;
    private float lerpSpeed = 10;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        
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
    }

    public void RotateUniverse(float a)
    {
        targetTransfrom.RotateAround(shipTransform.position, Vector3.up, -a * Time.deltaTime);
    }
}
