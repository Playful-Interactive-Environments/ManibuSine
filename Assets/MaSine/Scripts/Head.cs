using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour {

    public Transform target;
    public Vector3 aimPoint;
    public LayerMask mask;
    private AudioManager audioManager;
    private AudioSource asource;

    UI_TargetingDot targetingDot;
    RectTransform targetingDotRect;

    NetworkPlayer player;

    // Use this for initialization
    void Start () {
        audioManager = AudioManager.Instance;
        player = GetComponentInParent<NetworkPlayer>();
        if (!player.isLocalPlayer)
            return;
        targetingDot = FindObjectOfType<UI_TargetingDot>();
        targetingDotRect = targetingDot.GetComponent<RectTransform>();

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

        if (aimPoint != Vector3.zero)
        {
            // apply scaling 
            targetingDotRect.localScale = Vector2.one * Mathf.Pow(hit.distance, 0.7f) / 100;
            targetingDotRect.position = aimPoint - transform.forward;
        }
        else
        {
            targetingDot.ResetDot();
        }
	}
}
