using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_PickUp : MonoBehaviour {

    private UI_TargetingDot targetingDot;
    private Image dotGfx;
    private Transform pickUp;
    private PickUpRay ray;
    private int playerID;

    private Image progressCircle;

    // public UI_Targeting targetingUI;

	void Start () {
        targetingDot = transform.parent.GetComponentInChildren<UI_TargetingDot>();
        dotGfx = targetingDot.GetComponent<Image>();

        progressCircle = GetComponentInChildren<Image>();

        PickUpRay.GotTarget += OnGotTarget;
        PickUpRay.LostTarget += OnLostTarget;

        

        InvokeRepeating("GetSteeringManager", 0.01f, 0.2f);
	}
    void Update() {
        AnimateUI();
    }
    private void AnimateUI()
    {
        if (ray == null || ray.pickUp == null)
            return;

        progressCircle.fillAmount = ray.PickUpProgress01;

        //targetingUI.AnimateUI(1 - ray.PickUpProgress01, pickUp);
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

    private void OnGotTarget(int playerID, PickUpRay ray)
    {
        //targetingUI.ShowGraphics(true);

        progressCircle.enabled = true;

        this.ray = ray;
        this.pickUp = ray.pickUp.transform;
    }

    private void OnLostTarget(int playerID)
    {
        progressCircle.enabled = false;
        //targetingUI.ShowGraphics(false);
    }

    private void OnEnterSteering(SteeringStation steeringStation)
    {
        //targetingUI.ActivatePickUpUI();
        if (!steeringStation.NetworkPlayer.isLocalPlayer)
            return;

        playerID = steeringStation.NetworkPlayer.gameObject.GetInstanceID();

        dotGfx.enabled = true;
    }
    private void OnExitSteering(SteeringStation steeringStation)
    {
        if (!steeringStation.NetworkPlayer.isLocalPlayer)
            return;

        progressCircle.enabled = false;
        //targetingUI.ShowGraphics(false);

        playerID = 0;

        dotGfx.enabled = false;
    }
}
