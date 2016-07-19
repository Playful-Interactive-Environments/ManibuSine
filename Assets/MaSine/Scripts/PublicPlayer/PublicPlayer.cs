using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PublicPlayer : NetworkBehaviour {
    private const float updateRate = 0.491f;
    private float currentUpdate = 0;
    private float lerpSpeed = 1;
    private bool doPosUpdateClient = false;

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
    [SyncVar]
    public uint id;

    private PublicPickUp pickUp;


    // fancy stuff
    private Material mat;
    private float animationSpeed = 3;
    private float animationDistance = 2.5f;
    private float animationCurrent = 0;
    private bool animationUp = true;


    void Start() {
        mat = GetComponent<MeshRenderer>().material;
        // only client
        if (isServer)
            return; 

        if (id != 0)
            GetPickUp(id);

        // client version needs no rigidbody - so delete it
        Rigidbody body = GetComponent<Rigidbody>();
        if (body == null)
            return;

        DestroyImmediate(body);
    }

    private void AnimateMaterial()
    {
        if (animationUp)
        {
            if (animationCurrent < animationDistance)
            {
                animationCurrent += Time.deltaTime * animationSpeed;
            }
            else
            {
                animationUp = false;
            }
        }
        else
        {
            if (animationCurrent > 0.5f)
            {
                animationCurrent -= Time.deltaTime * animationSpeed;
            }
            else
            {
                animationUp = true;
            }
        }

        mat.mainTextureScale = Vector2.one * animationCurrent;
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

        AnimateMaterial();
    }

    [ClientRpc]
    private void RpcGetPickUp(uint id) {
        GetPickUp(id);
    }

    private void GetPickUp(uint id)
    {
        PublicPickUp[] pickUps = FindObjectsOfType<PublicPickUp>();

        foreach (PublicPickUp item in pickUps)
        {
            if (item.netId.Value == id)
            {
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
        {
            id = p.netId.Value;
            RpcGetPickUp(p.netId.Value);
        }
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
        id = 0;
        if (pickUp == null)
            return;

        pickUp.Player = null;
    }
}
