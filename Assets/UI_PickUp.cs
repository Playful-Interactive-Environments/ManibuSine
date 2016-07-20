using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_PickUp : MonoBehaviour {

    private UI_TargetingDot targetingDot;
    private Image dotGfx;
    private NetworkPlayer player;

	void Start () {
        targetingDot = transform.parent.GetComponentInChildren<UI_TargetingDot>();
        dotGfx = targetingDot.GetComponent<Image>();

        InvokeRepeating("GetSteeringManager", 0.5f, 0.5f);
	}

    void GetSteeringManager()
    {
        SteeringStation steeringManager = FindObjectOfType<SteeringStation>();
        if (steeringManager != null)
        {
            CancelInvoke("GetSteeringManager");
            steeringManager.EnteredSteering += OnEnterSteering;
            steeringManager.ExitedSteering += OnExitSteering;
        }
    }

    private void OnEnterSteering(SteeringStation steeringStation)
    {
        if (!steeringStation.NetworkPlayer.isLocalPlayer)
            return;

        dotGfx.enabled = true;
    }
    private void OnExitSteering(SteeringStation steeringStation)
    {
        if (!steeringStation.NetworkPlayer.isLocalPlayer)
            return;

        dotGfx.enabled = false;
    }
}
