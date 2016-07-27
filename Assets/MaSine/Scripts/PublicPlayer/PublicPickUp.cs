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

    void OnMouseDrag()
    {
        print(this.GetType().Name + ": " + "simulate pick");
        //PickIt();
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
        Collider[] col = GetComponentsInChildren<Collider>();
        foreach (Collider item in col)
            item.enabled = false;

    }

    private void PositionUpdate() {
        if (player == null)
            return;

        //transform.position = Vector3.Lerp(transform.position, player.transform.position, lerpSpeed * Time.deltaTime);
        transform.position = player.transform.position + Vector3.down;
    }

	void Update () {
        PositionUpdate();
	}
}
