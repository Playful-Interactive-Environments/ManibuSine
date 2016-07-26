using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PublicPickUp : NetworkBehaviour {
    private float lerpSpeed = 1;
    private float minDistance = 1.3f;
    public GameObject pickUpParticles;
    public Material on, off;
    private MeshRenderer meshRenderer;
    private AudioSource audio;

    private PublicPlayer player;
    public PublicPlayer Player {
        get {
            return player;
        }

        set {
            player = value;

            if (value == null) {
                meshRenderer.material = off;
            } else {
                meshRenderer.material = on;
            }
        }
    }

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start() {
        //meshRenderer = GetComponent<MeshRenderer>();

        audio = GetComponent<AudioSource>();
        transform.parent = UniverseTransformer.Instance.transform;
    }

    void OnMouseDown()
    {
        print(this.GetType().Name + ": " + "simulate pick");
        PickIt();
    }

    public void PickIt()
    {
        if (player == null)
            return;

        audio.pitch = Random.Range(0.9f, 1.1f);
        audio.Play();

        Instantiate(pickUpParticles, transform.position, Quaternion.identity);

        player.PickedUp();

        // disable rendering and collider (trigger)
        meshRenderer.enabled = false;
        Collider col = GetComponentInChildren<Collider>();
        if (col != null)
            col.enabled = false;

        // delete line renderer
        if (isServer)
            return;
    }

    private void PositionUpdate() {
        if (player == null || Vector3.Distance(transform.position, player.transform.position) < minDistance)
            return;

        transform.position = Vector3.Lerp(transform.position, player.transform.position, lerpSpeed * Time.deltaTime);
    }

	void Update () {
        PositionUpdate();
	}
}
