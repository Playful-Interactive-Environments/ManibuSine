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
    private Image[] graphics;

    private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        InitializeUI();
        InvokeRepeating("GetCanonManager", 0.5f, 0.5f);
	}

    void InitializeUI()
    {
        rectTransform = GetComponent<RectTransform>();
        targetSize = rectTransform.sizeDelta.x;
        graphics = GetComponentsInChildren<Image>();
        ShowGraphics(false);
    }

    void ShowGraphics(bool enable)
    {
        foreach (Image item in graphics)
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
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (hasTarget)
        {
            float size = (1 - t) * maxSize;
            rectTransform.sizeDelta = new Vector2(targetSize + size, targetSize + size);
        }
	}

    private void GotTarget(CanonManager canonManager)
    {
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
    }

}
