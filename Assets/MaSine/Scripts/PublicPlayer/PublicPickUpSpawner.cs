using UnityEngine;
using System.Collections;

public class PublicPickUpSpawner : MonoBehaviour {

    public PublicPickUp pickUpPrefab;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnPU", 0.5f, 3);
	}
	
	// Update is called once per frame
	void SpawnPU () {
        ServerManager.Instance.SpawnEntityAt(pickUpPrefab.gameObject, new Vector3(Random.Range(0, 9), Random.Range(3, 4), Random.Range(4, 9)), Quaternion.identity);
	}
}
