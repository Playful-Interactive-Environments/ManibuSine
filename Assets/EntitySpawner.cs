using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EntitySpawner : MonoBehaviour {

    ServerManager serverManager;

    public GameObject asteroidPrefab;

	// Use this for initialization
	void Start () {
        serverManager = GetComponent<ServerManager>();

        Invoke("SpawnThat", 6);
	}

    void SpawnThat()
    {
        serverManager.SpawnEntity(asteroidPrefab);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
