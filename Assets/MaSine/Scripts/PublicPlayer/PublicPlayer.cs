using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PublicPlayer : NetworkBehaviour {
    private const float updateRate = 0.1f;
    private float currentUpdate = 0;
    private float lerpSpeed = 5;
    //private bool doPosUpdateClient = false;

    private MeshRenderer mr;

    public Material lineMaterial;
    private LineRenderer lineRenderer;


    private MaSineTrackedPlayer controllingPlayer;
    public MaSineTrackedPlayer ControllingPlayer {
        get {
            return controllingPlayer;
        }

        set {
            controllingPlayer = value;
        }
    }

    //[SyncVar(hook = "FirstPositionDataX")]
    //private float x;
    //[SyncVar]
    //private float y;
    //[SyncVar]
    //private float z;
    [SyncVar]
    public uint id;

    private PublicPickUp pickUp;

    void Start() {
        mr = GetComponent<MeshRenderer>();
        Color randomColor = Color.HSVToRGB(Random.Range(0.0f, 0.999f), 0.999f, 0.2f);
        randomColor = new Color(randomColor.r, randomColor.g, randomColor.b, 0.7f);
        mr.materials[0].SetColor("_TintColor", randomColor);

        // only client
        if (isServer)
            return;

        if (id != 0)
            GetPickUp(id);

        // client version needs no rigidbody - so delete it
        Rigidbody body = GetComponent<Rigidbody>();
        if (body == null)
            return;

        //body.useGravity = false;

        //        DestroyImmediate(body);

    }

    //private void FirstPositionDataX(float val) {
    //    x = val;

    //    // do not update before first values has been sent from server
    //    if (!doPosUpdateClient)
    //        doPosUpdateClient = true;
    //}

    //private void UpdatePosition() {
    //    if (currentUpdate < updateRate) {
    //        currentUpdate += Time.deltaTime;
    //    }
    //    else {
    //        currentUpdate = 0;
    //        // set sync vars
    //        x = transform.position.x;
    //        y = transform.position.y;
    //        z = transform.position.z;
    //    }
    //}

    private void UpdatePickUpPosition() {
        if (pickUp == null)
            return;
    }

    void Update() {
        if (isServer) {
            if (controllingPlayer == null)
                return;
            transform.position = new Vector3(controllingPlayer.transform.position.x, transform.position.y, controllingPlayer.transform.position.z);

            //UpdatePosition();
            //UpdateLine();
        }
        else { // movement on clint
            //if (doPosUpdateClient)
            //    transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, z), lerpSpeed * Time.deltaTime);
        }
        UpdateLine();
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

        //if (isServer)
        //{
        id = p.netId.Value;
        if (isServer)
            RpcGetPickUp(p.netId.Value);

        MakeLine();
        //}
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

    public void PickedUp()
    {
        // delete line renderer
        if (isServer)
        {
            LineRenderer lr = GetComponent<LineRenderer>();

            if (lr != null)
                Destroy(lr);
        }
    }

    // visualize connection

    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    void MakeLine()
    {
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.material = lineMaterial; //new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(c1, c2);
        lineRenderer.SetWidth(0.5F, 0.2F);
        lineRenderer.SetVertexCount(2);
    }
    void UpdateLine()
    {
        if (pickUp == null || lineRenderer == null)
            return;

        print("HAS pickup " + pickUp);
        //LineRenderer lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, pickUp.transform.position);
    }
}