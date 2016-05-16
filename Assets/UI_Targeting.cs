using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Targeting : MonoBehaviour {

    //[Range(0,1)]
    private float t = 0;
    private float targetSize;
    private float maxSize = 200.0f;

    private bool hasTarget = false;

    private CanonManager canonManager;

    private Image[] targetGraphics;
    public Image dotGraphic;

    private RectTransform rectTransform;
    RectTransform CanvasRect;

	// Use this for initialization
	void Start () {
        InitializeUI();
        InvokeRepeating("GetCanonManager", 0.5f, 0.5f);

        CanvasRect = transform.parent.GetComponent<RectTransform>();
	}

    void InitializeUI()
    {
        rectTransform = GetComponent<RectTransform>();
        targetSize = rectTransform.sizeDelta.x;
        targetGraphics = GetComponentsInChildren<Image>();
        ShowGraphics(false);
    }

    void ShowGraphics(bool enable)
    {
        foreach (Image item in targetGraphics)
            item.enabled = enable;
    }

    // assigns the canon manager
    void GetCanonManager()
    {
        canonManager = FindObjectOfType<CanonManager>();
        if (canonManager != null)
        {
            CancelInvoke("GetCanonManager");
            canonManager.GotTarget += GotTarget;
            canonManager.LostTarget += LostTarget;
            canonManager.EnteredCannon += EnteredCannon;
            canonManager.ExitCannon += ExitCannon;
        }
    }
	
    // TODO: change position calculation - because now we are using world canvas
	void Update () {
        if (hasTarget)
        {
            float size = (1 - Mathf.Clamp01(canonManager.TargetedTime)) * maxSize;
            rectTransform.sizeDelta = new Vector2(targetSize + size, targetSize + size);

            if (canonManager.TargetTransform != null) {
                Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(canonManager.TargetTransform.position);

                Vector2 WorldObject_ScreenPosition = new Vector2(
                                                        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                                                        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

                rectTransform.anchoredPosition = WorldObject_ScreenPosition;
            }
            else
            {
                ShowGraphics(false);
            }
        }
	}

    private void EnteredCannon(CanonManager canonManager)
    {
        if (canonManager.IsGunnerLocalPlayer())
            dotGraphic.enabled = true;
    }
    private void ExitCannon(CanonManager cannonManager)
    {
        if (canonManager.IsGunnerLocalPlayer())
            dotGraphic.enabled = false;
    }

    private void GotTarget(CanonManager canonManager)
    {
        if (!canonManager.IsGunnerLocalPlayer())
            return;

        hasTarget = true;
        ShowGraphics(true);
    }

    private void LostTarget()
    {
        hasTarget = false;
        ShowGraphics(false);
    }

    void Dispose()
    {
        canonManager.GotTarget -= GotTarget;
        canonManager.LostTarget -= LostTarget;
    }
}