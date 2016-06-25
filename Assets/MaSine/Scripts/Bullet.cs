using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

    public float speed = 1000.0f;
    public float lifeTime = 10.0f;

    // Use this for initialization
    void Start () {
        transform.parent = UniverseTransformer.Instance.transform;
        //GetComponent<Rigidbody>().velocity = transform.up * speed;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (isServer)
        {
            Destroy(gameObject);
        }
    }
}
