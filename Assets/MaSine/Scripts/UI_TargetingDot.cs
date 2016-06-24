using UnityEngine;
using System.Collections;

public class UI_TargetingDot : MonoBehaviour {

    private float initialDistance;
    private float initialScale;
    private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        initialDistance = rectTransform.position.z;
        initialScale = rectTransform.localScale.x;
	}

    public void ResetDot()
    {
        rectTransform.localPosition = new Vector3(0, 0, initialDistance);
        rectTransform.localScale = Vector3.one * initialScale;
    }
}
