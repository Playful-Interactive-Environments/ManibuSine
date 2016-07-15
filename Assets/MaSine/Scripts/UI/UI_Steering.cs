﻿using UnityEngine;
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
    void GetSteeringManager()
    {
        steeringManager = FindObjectOfType<SteeringStation>();
        if (steeringManager != null)
        {
            CancelInvoke("GetSteeringManager");
            steeringManager.EnteredSteering += OnEnteredSteering;
            steeringManager.ExitedSteering += OnExitedSteering;
        }
    }

    Vector3 oldPos;
    void Update() {
        if (steeringManager == null || steeringManager.navigator == null)
            return;

        if (Mathf.Abs(steeringManager.angleInput) > 90) {
            return;
        }

        // better not
        //ShowGraphics(true);

        AnimateArrow();
        AnimateSpeed();
    }
    
    private void AnimateArrow()
    {
        float ls = lerpSpeed * Time.deltaTime;

        rectArrow.localRotation = Quaternion.Lerp(rectArrow.localRotation, Quaternion.Euler(0, 0, steeringManager.angleInput), ls);
    }

    private void AnimateSpeed()
    {
        float ls = lerpSpeed * Time.deltaTime;

        float clampedSpeed = Mathf.Clamp01(steeringManager.uiSpeedScale);
        speedBar.localScale = Vector3.Lerp(speedBar.localScale, new Vector3(1, clampedSpeed, 1), ls);
    }
    
    private void OnEnteredSteering(SteeringStation steeringStation) {
        ShowGraphics(true);
    }

    private void OnExitedSteering(SteeringStation steeringStation) {
        ShowGraphics(false);
    }

    void ShowGraphics(bool enable) {
        foreach (Image item in allGraphics)
            item.enabled = enable;

        UI_DEBUG.AddText("ShowGraphics " + enable);
    }

    void InitializeUI() {
        rectSteeringCircle = GetComponent<RectTransform>();
        originalScale = rectSteeringCircle.localScale;

        allGraphics = GetComponentsInChildren<Image>();
        ShowGraphics(false);
    }

    void OnDestroy()
    {
        if (steeringManager == null)
            return;
        steeringManager.EnteredSteering -= OnEnteredSteering;
        steeringManager.ExitedSteering -= OnExitedSteering;
    }
}