using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UI_PickUpProgressFloor : MonoBehaviour {

    private PickUpRay ray;
    private Image progressCircle;

	// Use this for initialization
	void Start () {
        progressCircle = GetComponentInChildren<Image>();
        PickUpRay.GotTarget += OnGotTarget;
        PickUpRay.LostTarget += OnLostTarget;
    }

    // Update is called once per frame
    void Update() {
        UpdateCircle();
    }

    private void UpdateCircle() {
        if (ray != null) {
            progressCircle.transform.position = ray.pickUp.transform.position;
            progressCircle.fillAmount = ray.PickUpProgress01;
        }
    }

    private void OnGotTarget(int picked, PickUpRay ray) {
        print(this.GetType().Name + ": GotTarget");
        this.ray = ray;
        progressCircle.enabled = true;
    }

    private void OnLostTarget(int picked) {
        this.ray = null;
        progressCircle.enabled = false;
    }

    void OnDestroy() {
        PickUpRay.GotTarget -= OnGotTarget;
        PickUpRay.LostTarget -= OnLostTarget;
    }

}
