using UnityEngine;
using System.Collections;

public class UniverseTransformer : MonoBehaviour {

    private static UniverseTransformer instance;
    public static UniverseTransformer Instance { get { return instance; } }

    public Transform transformTarget;
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

        transform.localPosition = Vector3.Lerp(transform.localPosition, transformTarget.localPosition, lerpSpeed *Time.deltaTime);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, transformTarget.rotation, lerpSpeed *Time.deltaTime);
	
	}

    public void MoveForward(float s)
    {
        transformTarget.Translate(transform.forward * -s * Time.deltaTime * 100);
    }

    public void RotateUniverse(float a)
    {
        transformTarget.RotateAround(transform.parent.parent.position, Vector3.up, -a *Time.deltaTime * 100);
    }
}
