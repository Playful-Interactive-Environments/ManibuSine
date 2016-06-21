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
	
	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        Physics.Raycast(ray, out hit, 1000, mask);

        target = hit.transform;
        aimPoint = hit.point;

        // only network player
        if (!player.isLocalPlayer)
            return;

        if (aimPoint != Vector3.zero)
        {
            float scaleFactor = Mathf.Pow(hit.distance, .83f) / 100;
            // apply scaling 
            targetingDotRect.localScale = Vector2.one * scaleFactor;
            targetingDotRect.position = aimPoint - transform.forward;
        }
        else
        {
            targetingDot.ResetDot();
        }
	}
}
