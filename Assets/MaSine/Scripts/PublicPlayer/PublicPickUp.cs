using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PublicPickUp : NetworkBehaviour {
    public float lifeTime = 30;
    public float alive = 0;

    private float lerpSpeed = 1;
    public GameObject pickUpParticles;
    public Material on, off;
    private MeshRenderer meshRenderer;
    private MeshRenderer crateMeshRenderer;
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
        crateMeshRenderer = GetComponentInChildren<MeshRenderer>();

        audio = GetComponent<AudioSource>();
        transform.parent = UniverseTransformer.Instance.transform;

        rotSpeed = Random.Range(-0.1f, 0.1f);
        if (rotSpeed < 0.04 && rotSpeed > -0.04)
            rotSpeed = 0.05f;

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
        crateMeshRenderer.enabled = false;
        Collider[] col = GetComponentsInChildren<Collider>();
        foreach (Collider item in col)
            item.enabled = false;

    }

    private void MeasureLivetime () {
        // only age if not carried by player
        if (player != null)
            return;

        if (alive < lifeTime) {
            // age
            alive += Time.deltaTime;
        } else {
            // destory
            Destroy(this.gameObject);
        }
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
        MeasureLivetime();
	}
}