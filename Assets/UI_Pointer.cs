using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_Pointer : MonoBehaviour {

    private SteeringStation steeringManager;
    private Transform pointAt;
    private RectTransform rectTrans;
    private Image arrow;

	void Start () {
        rectTrans = GetComponent<RectTransform>();
        arrow = GetComponentInChildren<Image>();

        WaypointLevel.NextWaypoint += OnGetNextWaypoint;
    }

    private void OnGetNextWaypoint(IEventTrigger waypoint) {
        pointAt = waypoint.GetTransform();
    }

    void Update () {
        if (pointAt == null)
            return;
        Vector3 dir = (pointAt.position - rectTrans.position).normalized;
        Vector3 final = dir - rectTrans.forward;

        float dotProd = Vector3.Dot(dir, final);

        if (dotProd > 1.9f || dotProd < 0.22f)
            arrow.enabled = false;
        else
            arrow.enabled = true;

        float a = (Mathf.Atan2(final.z, final.y)) * Mathf.Rad2Deg;
        rectTrans.localRotation = Quaternion.Euler(0,0,a);
    }

    void OnDestroy() {
        WaypointLevel.NextWaypoint -= OnGetNextWaypoint;
    }

    private void OnExitedSteering(SteeringStation steeringStation) {

    }

    private void OnEnteredSteering(SteeringStation steeringStation) {

    }
}