using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Pointer : MonoBehaviour {

    public Transform pointAt;
    private RectTransform rectTrans;
    private Image arrow;

	// Use this for initialization
	void Start () {
        rectTrans = GetComponent<RectTransform>();
        arrow = GetComponentInChildren<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 dir = (pointAt.position - rectTrans.position).normalized;
        Vector3 final = dir - rectTrans.forward;


        float a = (Mathf.Atan2(final.x, final.y)) * Mathf.Rad2Deg;
        print(a);
        rectTrans.localRotation = Quaternion.Euler(0,0,a);
    }

}
