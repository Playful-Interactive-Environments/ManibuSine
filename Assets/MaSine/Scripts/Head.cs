using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour {

    public Transform target;
    public Vector3 aimPoint;
    public LayerMask mask;
    private AudioManager audioManager;
    private AudioSource asource;

    // Use this for initialization
    void Start () {
        audioManager = AudioManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        Physics.Raycast(ray, out hit, 20000, mask);

        target = hit.transform;
        aimPoint = hit.point;
	}
}
