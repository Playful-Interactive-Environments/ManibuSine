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
        if (transformTarget == null)
            return;

        this.transform.position = transformTarget.position;
        this.transform.rotation = transformTarget.rotation;

    }

    public void MoveForward(float s)
    {
        transformTarget.Translate(transform.forward * -s * Time.deltaTime);
    }

    public void RotateUniverse(float a)
    {
        transformTarget.RotateAround(transform.parent.position, Vector3.up, -a * Time.deltaTime);
    }
}
