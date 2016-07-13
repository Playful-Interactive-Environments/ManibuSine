using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Targeting : MonoBehaviour {

    //[Range(0,1)]
    private float t = 0;
    private float targetSize;
    private float maxSize = 80.0f;

    private bool hasTarget = false;

    private CanonManager cannonManager;

    private Image[] targetGraphics;
    public UI_TargetingDot targetingDot;

    private RectTransform rectTransform;

    private Vector2 originalScale;

    // Use this for initialization
    void Start() {
        InitializeUI();

        CanonManager.GotTarget += OnGotTarget;
        CanonManager.LostTarget += OnLostTarget;
        CanonManager.EnteredCannon += OnEnteredCannon;
        CanonManager.ExitCannon += OnExitCannon;
    }

    void InitializeUI() {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        targetingDot = transform.parent.GetComponentInChildren<UI_TargetingDot>();

        targetSize = rectTransform.sizeDelta.x;
        targetGraphics = GetComponentsInChildren<Image>();
        ShowGraphics(false);
    }

    void ShowGraphics(bool enable)
    {
        foreach (Image item in targetGraphics)
            item.enabled = enable;
    }

    void Update()
    {
        if (!hasTarget || cannonManager == null)
        {
            ShowGraphics(false);
            return;
        }

        float size = (1 - Mathf.Clamp01(cannonManager.TargetedTime)) * maxSize;
        rectTransform.sizeDelta = Vector2.one * (targetSize + size);

        if (cannonManager.TargetTransform != null)
        {
            // set position to target
            transform.position = cannonManager.TargetTransform.position - transform.forward * 5;
            // calculate distance
            float distance = Vector3.Distance(transform.parent.position, cannonManager.TargetTransform.position);
            // wheight scaling factor on distance
            float scaleFactor = Mathf.Pow(distance, .8f) / 100;
            // apply scaling 
            rectTransform.localScale = originalScale + Vector2.one * scaleFactor;
        }
    }

    private void OnEnteredCannon(CanonManager canonManager)
    {
        if (!canonManager.IsGunnerLocalPlayer())
            return;

        targetingDot.Show(canonManager.netId.Value);
        this.cannonManager = canonManager;
    }
    private void OnExitCannon(CanonManager cannonManager)
    {
        if (this.cannonManager.IsGunnerLocalPlayer())
            targetingDot.Hide(cannonManager.netId.Value);

        hasTarget = false;
        this.cannonManager = null;
    }

    private void OnGotTarget(CanonManager canonManager)
    {
        if (!canonManager.IsGunnerLocalPlayer())
            return;

        hasTarget = true;
        ShowGraphics(true);
    }

    private void OnLostTarget(uint id)
    {
        hasTarget = false;
        ShowGraphics(false);
    }

    void OnDestroy()
    {
        CanonManager.GotTarget -= OnGotTarget;
        CanonManager.LostTarget -= OnLostTarget;
        CanonManager.EnteredCannon -= OnEnteredCannon;
        CanonManager.ExitCannon -= OnExitCannon;
    }
}