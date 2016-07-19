using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PublicPickUp : NetworkBehaviour {
    private static int pickUpCounter = 0;

    public int id;
    private float lerpSpeed = 1;
    private float minDistance = 1.3f;

    public Material on, off;
    private MeshRenderer meshRenderer;

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

    void Start() {
        id = ++pickUpCounter;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void PickIt()
    {

        if (player == null)
            return;
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
