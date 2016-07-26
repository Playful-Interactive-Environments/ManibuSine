﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class UI_PickUpProgressServer : MonoBehaviour {

    private PickUpRay ray;
    private Image progressCircle;

	// Use this for initialization
	void Start () {
        progressCircle = GetComponent<Image>();
        InvokeRepeating("GetSteeringManager", 0.5f, 0.5f);
    }
    void GetSteeringManager() {
        SteeringStation steeringManager = FindObjectOfType<SteeringStation>();
        if (steeringManager != null) {
            CancelInvoke("GetSteeringManager");
            //steeringManager.EnteredSteering += OnEnterSteering;
            //steeringManager.ExitedSteering += OnExitSteering;
            PickUpRay.GotTarget += OnGotTarget;
            PickUpRay.LostTarget += OnLostTarget;
        }
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
        this.ray = ray;
        progressCircle.enabled = true;
    }

    private void OnLostTarget(int picked) {
        this.ray = null;
        progressCircle.enabled = false;
    }


}
