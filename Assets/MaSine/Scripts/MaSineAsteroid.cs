using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MaSineAsteroid : NetworkBehaviour {

    NetworkPlayer player;
    private float speed = 10000.0f;
    private AudioManager audioManager;

	// Use this for initialization
	void Start () {
        
        audioManager = AudioManager.Instance;
        if (isServer)
            GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * speed);
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
        audioManager.PlayClipAt(audioManager.clips[0], transform.position);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
