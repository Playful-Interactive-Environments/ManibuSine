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
    public int id;


    // Use this for initialization
    void Start () {
        entitySpawner = GetComponent<EntitySpawner>();
        AsteroidEventTrigger.ShipEnteredEventID += VolumeEntered;
        AsteroidEventTrigger.ShipLeftEventID += VolumeExited;
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

    void Dispose()
    {
        AsteroidEventTrigger.ShipEnteredEventID -= VolumeEntered;
        AsteroidEventTrigger.ShipLeftEventID -= VolumeExited;
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

    private void VolumeExited(IEventTrigger waypoint, int[] spawnerId)
    {
        if (waypoint is AsteroidEventTrigger)
        {
            for(int i = 0; i < spawnerId.Length; i++)
            {
                if (spawnerId[i] == id)
                    Spawning = false;
            }
        }   
    }

    private void VolumeEntered(IEventTrigger waypoint, int[] spawnerId)
    {
        if (waypoint is AsteroidEventTrigger)
        {
            for (int i = 0; i < spawnerId.Length; i++)
            {
                if (spawnerId[i] == id)
                    Spawning = true;
            }
        }
    }
}
