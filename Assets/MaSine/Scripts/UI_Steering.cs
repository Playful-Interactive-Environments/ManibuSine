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

        rectArrow.localRotation = Quaternion.Euler(0, 0, -steeringManager.angleInput);
        rectArrow.localScale = new Vector3(1, Mathf.Clamp01(steeringManager.speedInput), 1);
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