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
        TrackedPlayer trackedPlayer = other.GetComponent<TrackedPlayer>();
        // is TrackedPlayer
        if (trackedPlayer == null)
            return;

        // allready has public player
        if (other.GetComponent<PublicPlayer>())
            return;

        other.gameObject.AddComponent<PublicPlayer>();

        // assign new player and parent it on tracked player
        ServerManager.Instance.SpawnPublicPlayer(trackedPlayer);


        //PublicPlayer newPP = Instantiate(publicPrefab);
        //newPP.controllingPlayer = trackedPlayer;
        //newPP.transform.parent = other.transform;
        //newPP.transform.localPosition = Vector3.zero;
    }
}
