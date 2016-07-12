using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour {

    public Transform target;
    public Vector3 aimPoint;
    public LayerMask mask;
    private AudioManager audioManager;
    private AudioSource asource;

    NetworkPlayer player;

    // Use this for initialization
    void Start () {
        audioManager = AudioManager.Instance;
        player = GetComponentInParent<NetworkPlayer>();
    }

    Ray ray;
	// Update is called once per frame
	void Update () {
        // only network player
        if (!player.isLocalPlayer)
            return;

        ray.origin = transform.position;
        ray.direction = transform.forward;
        RaycastHit hit;
        
        Physics.Raycast(ray, out hit, 1000, mask);

        target = hit.transform;
        aimPoint = hit.point;
	}
}
