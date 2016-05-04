using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MaSineAsteroid : NetworkBehaviour {

    NetworkPlayer player;

	// Use this for initialization
	void Start () {
        NetworkPlayer[] players = FindObjectsOfType<NetworkPlayer>();

        foreach (NetworkPlayer item in players)
        {
            if (item.isLocalPlayer)
                player = item;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        player.CmdDestroyEntity(gameObject);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
