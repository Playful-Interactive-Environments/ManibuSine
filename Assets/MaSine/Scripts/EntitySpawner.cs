using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class EntitySpawner : MonoBehaviour {

    ServerManager serverManager;
    
    public bool doSpawn = true;

	// Use this for initialization
	void Start () {
        ShipManager.GameOver += OnGameOver;
        serverManager = FindObjectOfType<ServerManager>();
	}

    private void OnGameOver(int damage)
    {
        doSpawn = false;
        this.enabled = false;
    }

    public void SpawnAt(GameObject entityToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (!doSpawn)
            return;

        serverManager.SpawnEntityAt(entityToSpawn, spawnPosition, spawnRotation);
    }

    public void SpawnAt(GameObject entityToSpawn)
    {
        if (!doSpawn)
            return;

        serverManager.SpawnEntityAtPrefabPosition(entityToSpawn);
    }

    public void OnDestroy()
    {
        ShipManager.GameOver -= OnGameOver;
    }
}
