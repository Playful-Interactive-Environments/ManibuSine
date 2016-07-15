using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PublicPlayer : NetworkBehaviour {

    public TrackedPlayer controllingPlayer;

    [SyncVar]
    float x;
    [SyncVar]
    float y;
    [SyncVar]
    float z;



    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

        if (isServer) {
            if (controllingPlayer != null)
                transform.position = new Vector3(controllingPlayer.transform.position.x, transform.position.y, controllingPlayer.transform.position.z);

            x = transform.position.x;
            y = transform.position.y;
            z = transform.position.z;
        }
        else {
            if (controllingPlayer != null)
                transform.position = new Vector3(x, y, z);
        }
    }
}
