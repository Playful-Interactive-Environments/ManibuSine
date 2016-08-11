using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PublicPickUp : NetworkBehaviour {
    private float lerpSpeed = 1;
    public GameObject pickUpParticles;
    public Material on, off;
    private MeshRenderer meshRenderer;
    private AudioSource audio;
    public Transform graphicTransform;
    private float rotSpeed;
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

        rotSpeed = Random.Range(-0.1f, 0.1f);
        if (rotSpeed < 0.01 && rotSpeed > -0.01)
            rotSpeed = 0.01f;

    }

    public void PickIt()
    {
        if (player == null)
            return;

        audio.pitch = Random.Range(0.98f, 1.12f);
        audio.Play();

        Instantiate(pickUpParticles, transform.position, Quaternion.identity);

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
        transform.position = player.transform.position + Vector3.down * 0.71f;
    }

	void Update () {
        graphicTransform.Rotate(rotSpeed, rotSpeed, rotSpeed);
        PositionUpdate();
	}
}
