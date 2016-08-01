using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_Pointer : MonoBehaviour {

    private SteeringStation steeringManager;
    private Transform pointAt;
    private Transform parentTrans;
    private Image arrow;

	void Start () {
        parentTrans = GetComponentInParent<Camera>().transform;
        arrow = GetComponentInChildren<Image>();

        WaypointLevel.NextWaypoint += OnGetNextWaypoint;
    }

    private void OnGetNextWaypoint(IEventTrigger waypoint) {
        pointAt = waypoint.GetTransform();
    }
    Vector3 dir, final;
    float dotProd, a;
    void Update () {
        if (pointAt == null)
            return;
        /*Vector3*/ dir = (pointAt.position - parentTrans.position).normalized;
        /*Vector3*/ final = (dir - parentTrans.forward).normalized;

        //final = new Vector3(final.x, final.y, final.z);
        /*float*/ dotProd = Vector3.Dot(dir, final);

        //if (dotProd > 1.9f || dotProd < 0.22f)
        //    arrow.enabled = false;
        //else
        //    arrow.enabled = true;


        /*float*/ a = (Mathf.Atan2(final.y, final.x)) * Mathf.Rad2Deg;
        //if (a <)

        //print("fin " + final);
        //print("dot " + dotProd);
        print("ang " + a);

        transform.localRotation = Quaternion.Euler(0,0,a - 90);
    }

    void OnDrawGizmos() {
        if (parentTrans == null)
            return;

        Debug.DrawRay(parentTrans.position, final * 10, Color.yellow);
    }

    void OnDestroy() {
        WaypointLevel.NextWaypoint -= OnGetNextWaypoint;
    }

    private void OnExitedSteering(SteeringStation steeringStation) {

    }

    private void OnEnteredSteering(SteeringStation steeringStation) {

    }
}