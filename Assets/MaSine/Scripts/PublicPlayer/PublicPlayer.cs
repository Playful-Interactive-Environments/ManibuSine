using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PublicPlayer : NetworkBehaviour {
    private const float updateRate = 0.1f;
    private float currentUpdate = 0;
    private float lerpSpeed = 5;

    private MeshRenderer mr;
    private AudioSource audioSource;

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

    [SyncVar]
    public uint id;

    private PublicPickUp pickUp;

    void Start() {
        audioSource = GetComponent<AudioSource>();
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
        if (isClient) {
            DestroyImmediate(body);
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
        }
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

        id = p.netId.Value;
        if (isServer)
            RpcGetPickUp(p.netId.Value);
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