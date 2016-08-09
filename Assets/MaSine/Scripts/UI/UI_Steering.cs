﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_Steering : MonoBehaviour {

    private SteeringStation steeringManager;

    private RectTransform rectSteeringCircle;
    public RectTransform rectArrow;
    public RectTransform speedBar;
    public Text speedtext;
    public Text angletext;

    private Vector2 originalScale;
    private Image[] allGraphics;
    private Text[] allText;

    private Transform unitverseTransTarget;

    private float lerpSpeed = 10;

    void Start() {
        InitializeUI();
        SteeringStation.EnteredSteering -= OnEnteredSteering;
        SteeringStation.ExitedSteering -= OnExitedSteering;
        SteeringStation.EnteredSteering += OnEnteredSteering;
        SteeringStation.ExitedSteering += OnExitedSteering;
    }

    public void AssignSteeringStation(SteeringStation station) {
        steeringManager = station;
        //SteeringStation.EnteredSteering += OnEnteredSteering;
        //SteeringStation.ExitedSteering += OnExitedSteering;

        // also tell pick up Ray
        PickUpRay ray = GetComponentInChildren<PickUpRay>();
        if (ray == null)
            return;
    }
    public void LogOfSteeringStation(SteeringStation station) {
        //SteeringStation.EnteredSteering -= OnEnteredSteering;
        //SteeringStation.ExitedSteering -= OnExitedSteering;
        steeringManager = null;
    }

    Vector3 oldPos;

    void Update() {
        if (steeringManager == null || steeringManager.navigator == null)
            return;

        if (Mathf.Abs(steeringManager.angleInput) > 90) {
            return;
        }

        AnimateArrow();
        AnimateSpeed();
    }
    
    private void AnimateArrow()
    {
        float ls = lerpSpeed * Time.deltaTime;

        rectArrow.localRotation = Quaternion.Lerp(rectArrow.localRotation, Quaternion.Euler(0, 0, steeringManager.angleInput), ls);
        float a = rectArrow.localRotation.eulerAngles.z;
        angletext.text = ((a > 180.0f) ? (360 - a + "°") : (a + "°"));
    }

    private void AnimateSpeed()
    {
        float ls = lerpSpeed * Time.deltaTime;

        float clampedSpeed = Mathf.Clamp01(steeringManager.uiSpeedScale);
        speedBar.localScale = Vector3.Lerp(speedBar.localScale, new Vector3(1, clampedSpeed, 1), ls);
        if (speedBar.localScale.y > 0.001f)
            speedtext.text = (800 * speedBar.localScale.y).ToString();
        else speedtext.text = "0.0";
    }
    
    private void OnEnteredSteering(SteeringStation steeringStation) {
        steeringManager = steeringStation;
        ShowGraphics(true);
    }

    private void OnExitedSteering(SteeringStation steeringStation) {
        ShowGraphics(false);
    }

    void ShowGraphics(bool enable) {
        foreach (Image item in allGraphics)
            item.enabled = enable;
        foreach (Text item in allText)
            item.enabled = enable;
    }

    void InitializeUI() {
        rectSteeringCircle = GetComponent<RectTransform>();
        originalScale = rectSteeringCircle.localScale;

        allGraphics = GetComponentsInChildren<Image>();
        allText = GetComponentsInChildren<Text>();
        ShowGraphics(false);
    }

    void OnDestroy()
    {
        if (steeringManager == null)
            return;
        SteeringStation.EnteredSteering -= OnEnteredSteering;
        SteeringStation.ExitedSteering -= OnExitedSteering;
    }
}