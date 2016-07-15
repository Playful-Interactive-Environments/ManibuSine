using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PublicPlayer : NetworkBehaviour {

    private const float updateRate = 0.2f;
    private float currentUpdate = 0;
    private float lerpSpeed = 12;

    public TrackedPlayer controllingPlayer;

    [SyncVar]
    public float x;
    [SyncVar]
    public float y;
    [SyncVar]
    public float z;



    // Use this for initialization
    void Start() {
        if (isServer) // only server
            InvokeRepeating("CheckPlayerGone", 1, 1);


        // only client
        if (isServer) 
            return;

        // client version needs no rigidbody
        Rigidbody body = GetComponent<Rigidbody>();
        if (body == null)
            return;

        DestroyImmediate(body);

    }

    void CheckPlayerGone() {
        if (controllingPlayer != null)
            return;
        CancelInvoke("CheckPlayerGone");
        DestroyImmediate(this.gameObject);
    }

    private void UpdatePosition() {
        if (currentUpdate < updateRate) {
            currentUpdate += Time.deltaTime;
        } else {
            currentUpdate = 0;
            // set sync vars
            // TODO: maybe use a sync rate
            x = transform.position.x;
            y = transform.position.y;
            z = transform.position.z;
        }
    }

    // Update is called once per frame
    void Update() {
        if (isServer) {

            if (controllingPlayer == null)
                return;
            transform.position = new Vector3(controllingPlayer.transform.position.x, transform.position.y, controllingPlayer.transform.position.z);

            UpdatePosition();

        }
        else { // movement on clint
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, z), lerpSpeed * Time.deltaTime);

            // TODO: maybe interpolate (Lerp)
        }
    }
}
