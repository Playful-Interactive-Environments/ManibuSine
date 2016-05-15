using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MaSineAsteroid : NetworkBehaviour {

    NetworkPlayer player;
    private float speed = 10000.0f;
    private AudioManager audioManager;
    private float rotSpeed;

    public GameObject explosionParticles;

	// Use this for initialization
	void Start () {
        
        audioManager = AudioManager.Instance;
        if (isServer) {
            GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * speed);
            rotSpeed = Random.Range(-0.05f, 0.05f) * 3.0f;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        //player.CmdDestroyEntity(gameObject);

        if (isServer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkDestroy()
    {
        audioManager.PlayClipAt(audioManager.clips[0], audioManager.sources[0], transform.position);

        if (!isServer)
            Instantiate(explosionParticles, transform.position, Quaternion.identity);
    }

	// Update is called once per frame
	void Update () {
        if (isServer)
            transform.Rotate(rotSpeed, rotSpeed, rotSpeed);
	}
}
