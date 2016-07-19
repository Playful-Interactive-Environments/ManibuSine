using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PublicPlayer : NetworkBehaviour {
    private const float updateRate = 0.491f;
    private float currentUpdate = 0;
    private float lerpSpeed = 1;
    private bool doPosUpdateClient = false;

    PublicPickUp pickUp;


    private MaSineTrackedPlayer controllingPlayer;
    public MaSineTrackedPlayer ControllingPlayer {
        get {
            return controllingPlayer;
        }

        set {
            controllingPlayer = value;
        }
    }

    [SyncVar(hook = "FirstPositionDataX")]
    private float x;
    [SyncVar]
    private float y;
    [SyncVar]
    private float z;

    void Start() {
        // only client
        if (isServer)
            return;

        // client version needs no rigidbody - so delete it
        Rigidbody body = GetComponent<Rigidbody>();
        if (body == null)
            return;

        DestroyImmediate(body);
    }

    private void FirstPositionDataX(float val) {
        x = val;

        // do not update before first values has been sent from server
        if (!doPosUpdateClient)
            doPosUpdateClient = true;
    }

    private void UpdatePosition() {
        if (currentUpdate < updateRate) {
            currentUpdate += Time.deltaTime;
        }
        else {
            currentUpdate = 0;
            // set sync vars
            x = transform.position.x;
            y = transform.position.y;
            z = transform.position.z;
        }
    }

    private void UpdatePickUpPosition() {
        if (pickUp == null)
            return;
    }

    void Update() {
        if (isServer) {

            if (controllingPlayer == null)
                return;
            transform.position = new Vector3(controllingPlayer.transform.position.x, transform.position.y, controllingPlayer.transform.position.z);

            UpdatePosition();

        }
        else { // movement on clint
            if (doPosUpdateClient)
                transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, z), lerpSpeed * Time.deltaTime);
        }
    }

    [ClientRpc]
    private void RpcAssignPickUp(int id) {
        PublicPickUp[] pickUps = FindObjectsOfType<PublicPickUp>();

        foreach (PublicPickUp item in pickUps) {
            if (item.id == id) {
                AssignPickUp(item);
                return;
            }
        }
    }

    private void AssignPickUp(PublicPickUp p) {
        // allready carries pickup
        if (pickUp != null)
            return;

        pickUp = p;
        p.Player = this;

        if (isServer)
            RpcAssignPickUp(p.id);
    }

    void OnTriggerEnter(Collider other) {
        if (!isServer)
            return;

        PublicPickUp p = other.GetComponent<PublicPickUp>();
        if (p == null)
            return;

        AssignPickUp(p);
    }



    void OnDestroy() {
        if (pickUp == null)
            return;

        pickUp.Player = null;
    }
}
