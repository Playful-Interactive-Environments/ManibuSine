using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_TargetingDot : MonoBehaviour {

    private float initialDistance;
    private float initialScale;
    private RectTransform rectTransform;
    private Image image;

    private Color originalColor;
    public Color denyColor;
    public Sprite dot, cross;

    private uint cannonID;

	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        initialDistance = rectTransform.position.z;
        initialScale = rectTransform.localScale.x;

        originalColor = image.color;

        CannonPivot.OutOfRange += OnOutOfRange;
        CannonPivot.InRange += OnInRange;
	}

    public void Show(uint cannonID)
    {
        this.cannonID = cannonID;
        image.enabled = true;
    }

    public void Hide()
    {
        image.enabled = false;
    }

    private void OnInRange()
    {
        image.color = originalColor;
        image.sprite = dot;
    }

    private void OnOutOfRange()
    {
        image.color = denyColor;
        image.sprite = cross;
    }

    public void ResetDot()
    {
        rectTransform.localPosition = new Vector3(0, 0, initialDistance);
        rectTransform.localScale = Vector3.one * initialScale;
    }

    void Dispose()
    {
        CannonPivot.OutOfRange -= OnOutOfRange;
        CannonPivot.InRange -= OnInRange;
    }
}
