using UnityEngine;
using System.Collections;

public class SpawnVolume : MonoBehaviour {

    public GameObject spawnObject;

    private EntitySpawner entitySpawner;

    private bool spawning;
    public bool Spawning
    {
        get
        {
            return spawning;
        }

        set
        {
            spawning = value;
        }
    }

    public float cooldown;
    private float currentCooldown = 0f;


    // Use this for initialization
    void Start () {
        entitySpawner = GetComponent<EntitySpawner>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!spawning)
            return;

        if (currentCooldown <= 0f)
        {
            SpawnObjectInVolume();
            currentCooldown = cooldown;
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    void SpawnObjectInVolume()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition += transform.right * Random.Range(-1f, 1f) * transform.lossyScale.x / 2f;
        spawnPosition += transform.up * Random.Range(-1f, 1f) * transform.lossyScale.y / 2f;
        spawnPosition += transform.forward * Random.Range(-1f, 1f) * transform.lossyScale.z / 2f;

        Quaternion spawnRotation = transform.rotation * Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

        entitySpawner.SpawnAt(spawnObject, spawnPosition, spawnRotation);
    }
}
