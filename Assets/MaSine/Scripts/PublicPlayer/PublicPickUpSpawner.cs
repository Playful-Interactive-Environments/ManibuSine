using UnityEngine;
using System.Collections;
using System;

public class PublicPickUpSpawner : MonoBehaviour {
    private static int pickUpCounter = 0;

    public PublicPickUp pickUpPrefab;

	// Use this for initialization
	void Start () {
        ShipManager.GameOver += OnGameOver;
        InvokeRepeating("SpawnPU", 0.1f, 20);
	}

    private void OnGameOver(int damage)
    {
        CancelInvoke("SpawnPU");
    }

    // Update is called once per frame
    void SpawnPU () {
        ServerManager.Instance.SpawnPickUp(new Vector3(4.68f, 1.96f, 8.4f));
	}
}
