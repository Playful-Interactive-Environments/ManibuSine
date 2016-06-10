using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EntitySpawner : MonoBehaviour {

    ServerManager serverManager;

    public float spawnRate = 4;
    public bool doSpawn = true;

	// Use this for initialization
	void Start () {
        serverManager = FindObjectOfType<ServerManager>();
	}

    public void SpawnAt(GameObject entityToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (!doSpawn)
            return;

        serverManager.SpawnEntityAt(entityToSpawn, spawnPosition, spawnRotation);
    }
}
