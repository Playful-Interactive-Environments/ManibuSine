using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_TargetingDot : MonoBehaviour {

    private float initialDistance;
    private float initialScale;
    private RectTransform rectTransform;
    private Image image;

    public Material originalMaterial;
    public Material denyMaterial;

    public Sprite dot, cross;

    private float dotSlace = 1;
    private float crossScale = 2;


    private uint cannonID;

	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        initialDistance = rectTransform.position.z;
        initialScale = rectTransform.localScale.x;

        CannonPivot.OutOfRange += OnOutOfRange;
        CannonPivot.InRange += OnInRange;
	}

    public void Show(uint cannonID)
    {
        this.cannonID = cannonID;

        image.sprite = dot;
        image.material = originalMaterial;
        rectTransform.localScale = Vector3.one * initialScale * dotSlace;

        image.enabled = true;
    }

    public void Hide(uint cannonID)
    {
        if (this.cannonID != cannonID)
            return;

        image.enabled = false;
    }

    private void OnInRange(uint cannonID)
    {
        if (this.cannonID != cannonID)
            return;
        image.sprite = dot;
        image.material = originalMaterial;

        rectTransform.localScale = Vector3.one * initialScale * dotSlace;
    }

    private void OnOutOfRange(uint cannonID)
    {
        if (this.cannonID != cannonID)
            return;
        image.sprite = cross;
        image.material = denyMaterial;
        rectTransform.localScale = Vector3.one * initialScale * crossScale;


        //ResetDot();
    }

    //public void ResetDot()
    //{
    //    rectTransform.localPosition = new Vector3(0, 0, initialDistance);
    //    rectTransform.localScale = Vector3.one * initialScale;
    //}

    void Dispose()
    {
        CannonPivot.OutOfRange -= OnOutOfRange;
        CannonPivot.InRange -= OnInRange;
    }
}