using UnityEngine;
using System.Collections;

// SERVER ONLY
public class PublicPlayerManager : MonoBehaviour {
    public PublicPlayer publicPrefab;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        MaSineTrackedPlayer trackedPlayer = other.GetComponent<MaSineTrackedPlayer>();
        // is TrackedPlayer
        if (trackedPlayer == null)
            return;

        // allready has public player
        if (trackedPlayer.PublicPlayer != null)
            return;

        // assign new player and parent it on tracked player
        ServerManager.Instance.SpawnPublicPlayer(trackedPlayer);
    }
}
