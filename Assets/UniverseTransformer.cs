using UnityEngine;
using System.Collections;

public class UniverseTransformer : MonoBehaviour {

    private static UniverseTransformer instance;
    public static UniverseTransformer Instance { get { return instance; } }

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

    public void MoveForward(float s)
    {
        this.transform.Translate(transform.forward * -s);
    }

    public void RotateUniverse(float a)
    {
        this.transform.localRotation = Quaternion.Euler(0, this.transform.localRotation.eulerAngles.y - a, 0);
    }
}
