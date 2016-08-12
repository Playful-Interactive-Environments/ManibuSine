using UnityEngine;
using System.Collections;

public class WallCamAnimator : MonoBehaviour {
    public bool animateRotation = true;
    public bool frontView = false;
    public bool resetRotation = false;
    private float rotationSpeed = 7;

    public bool animateDistance = true;
    public bool resetDistance = false;
    private float distance = 5;
    private float distanceSpeed = 0.33f;
    private float currentDistance = 1;

    private Vector3 initPos;
    private Quaternion initRot;

    private Vector3 camInitPos;
    private Quaternion camInitRot;
    private Camera cam;

    // Use this for initialization
    void Start() {
        initPos = transform.position;
        initRot = transform.rotation;
        cam = GetComponentInChildren<Camera>();
        camInitPos = cam.transform.position;
        camInitRot = cam.transform.rotation;
    }

    public void StartRotation() {
        resetRotation = false;
        animateRotation = true;
    }
    public void ResetRotation() {
        resetRotation = true;
        animateRotation = false;
    }
    public void StartDistance() {
        resetDistance = false;
        animateDistance = true;
    }
    public void ResetDistance() {
        resetDistance = true;
        animateDistance = false;
    }
    //public void GoToFrontView() {

    //}
    public void ResetAll() {
        ResetDistance();
        ResetRotation();
    }



    private void RotationUpdate() {
        if (animateRotation && !frontView) {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        } else if (animateRotation && frontView) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,180,0) , Time.deltaTime * 5);
        }
        else if (resetRotation) {
            transform.rotation = Quaternion.Lerp(transform.rotation, initRot, Time.deltaTime * 5);
            if (Mathf.Abs(Mathf.Abs(transform.rotation.eulerAngles.y) - Mathf.Abs(initRot.y)) < 0.001f)
                resetRotation = false;
        }
    }

    private void DistanceUpdate() {
        if (animateDistance) {
            transform.position = Vector3.Lerp(transform.position, new Vector3(initPos.x, initPos.y, initPos.z + (Mathf.Sin(currentDistance) - 1)), Time.deltaTime);
            currentDistance += distanceSpeed * Time.deltaTime;
        } else if (resetDistance) {
            transform.position = Vector3.Lerp(transform.position, initPos, Time.deltaTime * 5);
            if (Mathf.Abs(Mathf.Abs(transform.position.z) - Mathf.Abs(initPos.z)) < 0.001f)
                resetDistance = false; 
        }
    }

    // Update is called once per frame
    void Update() {
        RotationUpdate();
        DistanceUpdate();

        if (ServerManager.Instance.IsClientConnected()) {
            ResetRotation();
            ResetDistance();
        } else if (!ServerManager.Instance.IsClientConnected()) {
            StartRotation();
            StartDistance();
        }
    }
}