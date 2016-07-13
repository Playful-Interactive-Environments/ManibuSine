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
    void GetSteeringManager()
    {
        steeringManager = FindObjectOfType<SteeringStation>();
        if (steeringManager != null)
        {
            CancelInvoke("GetSteeringManager");
            steeringManager.EnteredSteering += OnEnteredSteering;
            steeringManager.ExitedSteering += OnExitedSteering;
            steeringManager.StepedOutSteering += OnStepedOutSteering;
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
        AnimateSpeed();
    }

    private void CountDown()
    {
        if (!doCountDown || stepOutCountdownText == null)
            return;

        stepOutDuration -= Time.deltaTime;
        stepOutCountdownText.text = stepOutDuration.ToString("#.");
    }

    private void AnimateArrow()
    {
        if (doCountDown)
            return;

        float ls = lerpSpeed * Time.deltaTime;
        float clampedSpeed = Mathf.Clamp01(steeringManager.uiArrowLength);

        rectArrow.localRotation = Quaternion.Lerp(rectArrow.localRotation, Quaternion.Euler(0, 0, steeringManager.angleInput), ls);
        //rectArrow.localScale = Vector3.Lerp(rectArrow.localScale, new Vector3(1, clampedSpeed, 1), ls);
    }

    private void AnimateSpeed()
    {
        if (doCountDown)
            return;

        float ls = lerpSpeed * Time.deltaTime;
        float clampedSpeed = Mathf.Clamp01(steeringManager.uiArrowLength);
        speedBar.localScale = Vector3.Lerp(speedBar.localScale, new Vector3(1, clampedSpeed, 1), ls);
    }

    private void StartCountdown()
    {
        doCountDown = true;
        if (stepOutCountdownText == null)
            return;
        stepOutCountdownText.enabled = true;
    }

    private void StopCountdown()
    {
        doCountDown = false;
        if (stepOutCountdownText == null)
            return;
        stepOutCountdownText.enabled = false;
    }

    private void OnEnteredSteering(SteeringStation steeringStation) {
        //steeringManager = steeringStation;
        StopCountdown();
        ShowGraphics(true);
    }

    private void OnStepedOutSteering(SteeringStation steeringStation)
    {
        StartCountdown();
        stepOutDuration = steeringStation.playerDropOutDelay;
        rectArrow.localScale = new Vector3(1, 1, 1);
    }

    private void OnExitedSteering(SteeringStation steeringStation) {
        //steeringManager = null;
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

    void OnDestroy()
    {
        if (steeringManager == null)
            return;
        steeringManager.EnteredSteering -= OnEnteredSteering;
        steeringManager.ExitedSteering -= OnExitedSteering;
        steeringManager.StepedOutSteering -= OnStepedOutSteering;
    }
}