using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_Steering : MonoBehaviour {

    private SteeringStation steeringManager;

    private RectTransform rectSteeringCircle;
    public RectTransform rectArrow;

    private Vector2 originalScale;
    private Image[] allGraphics;

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

    void Update() {
        if (steeringManager == null || steeringManager.navigator == null)
            return;

        if (Mathf.Abs(steeringManager.angleInput) > 90)
            return;

        float clampedSpeed = Mathf.Clamp01(steeringManager.speedInput);

        rectArrow.localRotation = Quaternion.Lerp(rectArrow.localRotation, Quaternion.Euler(0, 0, -steeringManager.angleInput), lerpSpeed * Time.deltaTime);
        rectArrow.localScale = Vector3.Lerp(rectArrow.localScale, new Vector3(1, clampedSpeed, 1), lerpSpeed * Time.deltaTime);
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