using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_Steering : MonoBehaviour {

    private SteeringStation steeringManager;

    private RectTransform rectSteeringCircle;
    public RectTransform rectArrow;
    public RectTransform speedBar;
    public Text stepOutCountdownText;
    private float stepOutDuration;
    private bool doCountDown;

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
            steeringManager.StepedOutSteering += StepedOutSteering;
        }
    }

    Vector3 oldPos;
    void Update() {
        if (steeringManager == null || steeringManager.navigator == null)
            return;

        CountDown();

        if (Mathf.Abs(steeringManager.angleInput) > 90) {
            return;
        }

        ShowGraphics(true);

        AnimateArrow();
    }

    private void CountDown()
    {
        if (!doCountDown)
            return;

        stepOutDuration -= Time.deltaTime;
        stepOutCountdownText.text = stepOutDuration.ToString("#.");
    }

    private void AnimateArrow()
    {
        float ls = lerpSpeed * Time.deltaTime;
        float clampedSpeed = Mathf.Clamp01(steeringManager.uiArrowLength);

        rectArrow.localRotation = Quaternion.Lerp(rectArrow.localRotation, Quaternion.Euler(0, 0, steeringManager.angleInput), ls);
        rectArrow.localScale = Vector3.Lerp(rectArrow.localScale, new Vector3(1, clampedSpeed, 1), ls);
    }

    private void StartCountdown()
    {
        if (stepOutCountdownText == null)
            return;
        stepOutCountdownText.enabled = true;
        doCountDown = true;
    }

    private void StopCountdown()
    {
        if (stepOutCountdownText == null)
            return;
        stepOutCountdownText.enabled = false;
        doCountDown = false;
    }

    private void EnteredSteering(SteeringStation steeringStation) {
        steeringManager = steeringStation;
        StopCountdown();
        ShowGraphics(true);
    }

    private void StepedOutSteering(SteeringStation steeringStation)
    {
        if (stepOutCountdownText != null)
        {
            StartCountdown();
            stepOutDuration = steeringStation.playerDropOutDelay;
            rectArrow.localScale = new Vector3(1, 0, 1);
        }
    }

    private void ExitedSteering(SteeringStation steeringStation) {
        steeringManager = null;
        StopCountdown();
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
        StopCountdown();
        ShowGraphics(false);
    }

    void Dispose()
    {
        steeringManager.EnteredSteering -= EnteredSteering;
        steeringManager.ExitedSteering -= ExitedSteering;
        steeringManager.StepedOutSteering -= StepedOutSteering;
    }
}