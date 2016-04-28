using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MaSineAsteroid : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        //if (isServer)
        //    Invoke("KillMe", 3);
	}

    [Command]
    void CmdKillMe()
    {
        NetworkServer.Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        print("KillMe");
        CmdKillMe();
    }


	// Update is called once per frame
	void Update () {
	
	}
}
