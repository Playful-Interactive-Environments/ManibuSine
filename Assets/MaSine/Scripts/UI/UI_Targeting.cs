using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Targeting : MonoBehaviour {
    //[Range(0,1)]
    private float t = 0;
    private float targetSize;
    private float maxSize = 80.0f;

    private float contextScale = 1;

    private bool hasTarget = false;

    public Sprite cannonSprite, pickupSprite;

    private CanonManager cannonManager;

    private Image[] targetGraphics;
    private Image[] TargetGraphics {
        get { return targetGraphics; }
    }

    private UI_TargetingDot targetingDot;

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

    private void ActivateCannonUI()
    {
        contextScale = 1;
        foreach (Image item in targetGraphics)
            item.sprite = cannonSprite;
    }

    public void ActivatePickUpUI()
    {
        contextScale = .2f;
        foreach (Image item in targetGraphics)
            item.sprite = pickupSprite;
    }

    public void ShowGraphics(bool enable)
    {
        foreach (Image item in targetGraphics)
            item.enabled = enable;
    }

    void Update()
    {
        if (!hasTarget || cannonManager == null)
        {
            //ShowGraphics(false);
            return;
        }

        AnimateUI((1 - Mathf.Clamp01(cannonManager.TargetedTime)), cannonManager.TargetTransform);
    }

    public void AnimateUI(float t, Transform target)
    {
        float size = t * maxSize;
        rectTransform.sizeDelta = Vector2.one * (targetSize + size);

        if (target != null)
        {
            // set position to target
            transform.position = target.position;
            // calculate distance
            float distance = Vector3.Distance(transform.parent.position, target.position);
            // wheight scaling factor on distance
            float scaleFactor = distance * 0.001f * target.localScale.x;
            // apply scaling 
            rectTransform.localScale = originalScale + Vector2.one * scaleFactor;
        }
    }

    private void OnEnteredCannon(CanonManager canonManager)
    {
        ActivateCannonUI();
        if (!canonManager.IsGunnerLocalPlayer())
            return;

        targetingDot.Show(canonManager.netId.Value);
        this.cannonManager = canonManager;
    }


    private void OnExitCannon(CanonManager cannonManager)
    {
        if (this.cannonManager == null)
            return; // ohter player left a station

        if (!cannonManager.IsGunnerLocalPlayer())
            return;


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