using UnityEngine;
using System.Collections;

public class PublicPickUpSpawner : MonoBehaviour {
    private static int pickUpCounter = 0;

    public float delayInSeconds;
    public float spawnRateInSeconds;
    public GameObject pickUpPrefab;
    private EntitySpawner entitySpawner;

	// Use this for initialization
	void Start () {
        ShipManager.GameOver += OnGameOver;
        InvokeRepeating("SpawnPU", delayInSeconds, spawnRateInSeconds);
        entitySpawner = GetComponent<EntitySpawner>();
	}

    private void OnGameOver(int damage)
    {
        CancelInvoke("SpawnPU");
    }

    // Update is called once per frame
    void SpawnPU () {
        Vector3 spawnPosition = transform.position;
        spawnPosition += transform.right * Random.Range(-1f, 1f) * transform.lossyScale.x / 2f;
        spawnPosition += transform.up * Random.Range(-1f, 1f) * transform.lossyScale.y / 2f;
        spawnPosition += transform.forward * Random.Range(-1f, 1f) * transform.lossyScale.z / 2f;

        // todo this is hardcoded on 16m width (deep space)
        if (spawnPosition.x > 5.5f && spawnPosition.x < 10.5f) {
            // move to left side
            if (spawnPosition.x < 8)
                spawnPosition.x -= 3;
            else
                spawnPosition.x += 3;
        }

        entitySpawner.SpawnAt(pickUpPrefab.gameObject, spawnPosition, new Quaternion());
	}

    void OnDestroy()
    {
        ShipManager.GameOver -= OnGameOver;
    }
}
