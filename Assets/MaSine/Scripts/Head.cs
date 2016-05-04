using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour {

    public Transform target;
    public LayerMask mask;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        Physics.Raycast(ray, out hit, 10000, mask);

        target = hit.transform;
	}
}
