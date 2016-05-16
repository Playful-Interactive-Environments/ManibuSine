using UnityEngine;
using System.Collections;

public class TransformTest : MonoBehaviour {

    public Transform target;
    public float speed = 100;

    public float rotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        target.RotateAround(transform.parent.position, Vector3.up, rotation);

        transform.position = target.position;
        transform.rotation = target.rotation;


        //transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, Time.deltaTime * speed);
        //transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * speed);
    }
}
