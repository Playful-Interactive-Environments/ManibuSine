using UnityEngine;
using System.Collections;

public class PublicPickUpSpawner : MonoBehaviour {
    private static int pickUpCounter = 0;

    public PublicPickUp pickUpPrefab;

	// Use this for initialization
	void Start () {

        InvokeRepeating("SpawnPU", 0.5f, 10);
	}
	
	// Update is called once per frame
	void SpawnPU () {
        ServerManager.Instance.SpawnPickUp(new Vector3(4.68f, 1.96f, 8.4f));
	}
}
