using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MaSineAsteroid : NetworkBehaviour {

    NetworkPlayer player;
    Transform ship;
    public static int destroyedAsteroids;
    public static float speed = 15.0f;
    public bool isStatic = false;
    private AudioManager audioManager;
    public float rotSpeed;
    private float originalScale;

    public Transform graphicTransform;
    public GameObject explosionParticles;
    private float destroyDistance = 350;
    bool silentDestruction = false;
    private float growSpeed = 0.3f;


    // Use this for initialization
    void Start () {
        originalScale = transform.localScale.x;
        transform.localScale = Vector3.zero;
        ship = GameObject.Find("Ship").transform;
        if (!isStatic)
            transform.parent = UniverseTransformer.Instance.transform;

        audioManager = AudioManager.Instance;

        rotSpeed = Random.Range(-0.1f, 0.1f);
        if (rotSpeed < 0.01 && rotSpeed > -0.01)
            rotSpeed = 0.01f;
	}

    void OnTriggerEnter(Collider other)
    {
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
        // shot by player
        if (!silentDestruction)
        {
            destroyedAsteroids++;
            audioManager.PlayClipAt(audioManager.clips[0], audioManager.sources[0], transform.position);
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
        }
    }

	// Update is called once per frame
	void Update () {
        if (transform.localScale.x < originalScale)
        {
            transform.localScale = new Vector3(transform.localScale.x + growSpeed, transform.localScale.y + growSpeed, transform.localScale.z + growSpeed);
        }

        graphicTransform.Rotate(rotSpeed, rotSpeed, rotSpeed);

        if (isServer)
        {
            if (!isStatic)
                transform.Translate(Vector3.forward * speed * Time.deltaTime);

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
