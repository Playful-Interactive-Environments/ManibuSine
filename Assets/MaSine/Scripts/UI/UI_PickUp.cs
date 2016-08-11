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
    

    void Start() {
        targetingDot = transform.parent.GetComponentInChildren<UI_TargetingDot>();

        dotGfx = targetingDot.GetComponent<Image>();

        progressCircle = GetComponentInChildren<Image>();

        PickUpRay.GotTarget += OnGotTarget;
        PickUpRay.LostTarget += OnLostTarget;

        SteeringStation.EnteredSteering += OnEnterSteering;
        SteeringStation.ExitedSteering += OnExitSteering;
        
    }
    void Update() {
        AnimateUI();
    }
    private void AnimateUI()
    {
        if (ray == null || ray.pickUp == null)
            return;

        progressCircle.fillAmount = ray.PickUpProgress01;
        
    }
    private void OnGotTarget(int playerID, PickUpRay ray)
    {
        dotGfx.sprite = targetingDot.dot;
        dotGfx.material = targetingDot.originalMaterial;
        progressCircle.enabled = true;
        progressCircle.fillAmount = 0;

        this.ray = ray;
        this.pickUp = ray.pickUp.transform;
    }

    private void OnLostTarget(int playerID)
    {
        progressCircle.enabled = false;
    }

    private void OnEnterSteering(SteeringStation steeringStation)
    {
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

        playerID = 0;

        dotGfx.enabled = false;
    }

    void OnDestroy() {
        PickUpRay.GotTarget -= OnGotTarget;
        PickUpRay.LostTarget -= OnLostTarget;
        SteeringStation.EnteredSteering -= OnEnterSteering;
        SteeringStation.ExitedSteering -= OnExitSteering;
    }
}
