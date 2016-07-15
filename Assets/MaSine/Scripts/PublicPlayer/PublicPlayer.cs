using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PublicPlayer : NetworkBehaviour {

    public TrackedPlayer controllingPlayer;

    [SyncVar]
    public float x;
    [SyncVar]
    public float y;
    [SyncVar]
    public float z;



    // Use this for initialization
    void Start() {
        if (isServer)
            return;

        // clietn version needs no rigidbody
        Rigidbody body = GetComponent<Rigidbody>();
        if (body == null)
            return;
        Destroy(body);


    }

    // Update is called once per frame
    void Update() {

        if (isServer) {
            if (controllingPlayer == null)
                Network.Destroy(this.gameObject);

            transform.position = new Vector3(controllingPlayer.transform.position.x, transform.position.y, controllingPlayer.transform.position.z);

            // set sync vars
            // TODO: maybe use a sync rate
            x = transform.position.x;
            y = transform.position.y;
            z = transform.position.z;
        }
        else { // movement on clint
            transform.position = new Vector3(x, y, z);

            // TODO: maybe interpolate (Lerp)
        }
    }
}
