using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MaSineAsteroid : NetworkBehaviour {

    NetworkPlayer player;
    Transform ship;
    public static float speed = 15.0f;
    private AudioManager audioManager;
    private float rotSpeed;

    public Transform graphicTransform;
    public GameObject explosionParticles;
    private float destroyDistance = 350;
    bool silentDestruction = false;


    // Use this for initialization
    void Start () {
        ship = GameObject.Find("Ship").transform;
        transform.parent = UniverseTransformer.Instance.transform;

        audioManager = AudioManager.Instance;
        if (isServer) {
            rotSpeed = Random.Range(-0.05f, 0.05f) * 3.0f;
        }

        
	}

    void OnTriggerEnter(Collider other)
    {
        //player.CmdDestroyEntity(gameObject);

        if (isServer)
        {
            if (!silentDestruction)
            {
                Instantiate(explosionParticles, transform.position, Quaternion.identity);
                audioManager.PlayClipAt(audioManager.clips[0], audioManager.sources[0], transform.position);
            }
            
            Destroy(gameObject);
        }
    }

    public override void OnNetworkDestroy()
    {
        if (!silentDestruction)
        {
            audioManager.PlayClipAt(audioManager.clips[0], audioManager.sources[0], transform.position);
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
        }
        
    }

	// Update is called once per frame
	void Update () {
        if (isServer)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            graphicTransform.Rotate(rotSpeed, rotSpeed, rotSpeed);

            if (Vector3.Distance(transform.position, ship.position) > destroyDistance)
            {
                silentDestruction = true;
                RpcSetClientSilent();
                Destroy(gameObject);
            }
        }
    }
    [ClientRpc]
    void RpcSetClientSilent()
    {
        silentDestruction = true;
    }
}
