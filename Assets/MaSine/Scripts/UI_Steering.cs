using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_Steering : MonoBehaviour {

    private SteeringStation steeringManager;

    private RectTransform rectSteeringCircle;
    public RectTransform rectArrow;
    public RectTransform speedBar;

    private Vector2 originalScale;
    private Image[] allGraphics;

    private Transform unitverseTransTarget;

    private float lerpSpeed = 10;

    void Start() {
        InitializeUI();
        InvokeRepeating("GetSteeringManager", 0.5f, 0.5f);
    }
    void GetSteeringManager() {
        steeringManager = FindObjectOfType<SteeringStation>();
        if (steeringManager != null) {
            CancelInvoke("GetSteeringManager");
            steeringManager.EnteredSteering += EnteredSteering;
            steeringManager.ExitedSteering += ExitedSteering;
        }
    }


    Vector3 oldPos;
    void Update() {
        if (steeringManager == null || steeringManager.navigator == null)
            return;

        if (Mathf.Abs(steeringManager.angleInput) > 90) {
            ShowGraphics(false);
            return;
        }

        ShowGraphics(true);

        float ls = lerpSpeed * Time.deltaTime;

        // speed indicator
        //if (speedBar != null) {
        //    Transform tt = UniverseTransformer.Instance.GetTargetTransform();
        //    if (tt != null) {
        //        float speed = Vector3.Distance(oldPos, tt.position) * 100;
        //        speedBar.localScale = Vector3.Lerp(speedBar.localScale, new Vector3(1, speed, 1), ls);
        //        print("SPEED " + speed);
        //        oldPos = tt.position;
        //    }

        //}


        float clampedSpeed = Mathf.Clamp01(steeringManager.uiArrowLength);

        rectArrow.localRotation = Quaternion.Lerp(rectArrow.localRotation, Quaternion.Euler(0, 0, -steeringManager.angleInput), ls);
        rectArrow.localScale = Vector3.Lerp(rectArrow.localScale, new Vector3(1, clampedSpeed, 1), ls);
    }

    private void EnteredSteering(SteeringStation steeringStation) {
        steeringManager = steeringStation;
        ShowGraphics(true);
    }

    private void ExitedSteering(SteeringStation steeringStation) {
        steeringManager = null;
        ShowGraphics(false);
    }

    void ShowGraphics(bool enable) {
        foreach (Image item in allGraphics)
            item.enabled = enable;
    }

    void InitializeUI() {
        rectSteeringCircle = GetComponent<RectTransform>();
        originalScale = rectSteeringCircle.localScale;

        allGraphics = GetComponentsInChildren<Image>();
        ShowGraphics(false);
    }
}