using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EntitySpawner : MonoBehaviour {

    ServerManager serverManager;

    public float spawnRate = 4;
    public bool doSpawn = true;

    public GameObject asteroidPrefab;

	// Use this for initialization
	void Start () {
        serverManager = GetComponent<ServerManager>();

        InvokeRepeating("SpawnThat", 6, spawnRate);
	}

    void SpawnThat()
    {
        if (!doSpawn)
            return;

        serverManager.SpawnEntity(asteroidPrefab);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
