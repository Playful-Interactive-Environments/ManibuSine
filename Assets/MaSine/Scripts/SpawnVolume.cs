using UnityEngine;
using System.Collections;

public class SpawnVolume : MonoBehaviour {

    public GameObject spawnObject;

    private EntitySpawner entitySpawner;

    [SerializeField]
    float randomDirectionOffset = 2.0f;

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
        // Aim spawnVolume at ship and ahead
        ShipManager shipMan = FindObjectOfType<ShipManager>();
        UniverseTransformTarget transTarget = FindObjectOfType<UniverseTransformTarget>();
        if(shipMan != null && transTarget != null)
        {
            float distance = Vector3.Distance(shipMan.transform.position, transform.position);
            transform.LookAt(shipMan.transform.position + (-transTarget.GetComponent<Rigidbody>().velocity) * (distance * (1f / MaSineAsteroid.speed)));
            //Debug.DrawRay(transform.position, transform.forward * 1000, Color.green, 100);
        }
        
        Vector3 spawnPosition = transform.position;
        spawnPosition += transform.right * Random.Range(-1f, 1f) * transform.lossyScale.x / 2f;
        spawnPosition += transform.up * Random.Range(-1f, 1f) * transform.lossyScale.y / 2f;
        spawnPosition += transform.forward * Random.Range(-1f, 1f) * transform.lossyScale.z / 2f;

        //Quaternion spawnRotation = Quaternion.Euler(FindObjectOfType<ShipManager>().transform.position - transform.position);
        Quaternion spawnRotation = transform.rotation * Quaternion.Euler(Random.Range(-randomDirectionOffset, randomDirectionOffset), 
                                                                        Random.Range(-randomDirectionOffset, randomDirectionOffset), 
                                                                        Random.Range(-randomDirectionOffset, randomDirectionOffset));

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
