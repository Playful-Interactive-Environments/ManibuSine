using UnityEngine;
using System.Collections;

public class VR_CylinderBorder : MonoBehaviour {

    private MeshRenderer mesh;
    private NetworkPlayer thisPlayer;
    public void AssignPlayer(NetworkPlayer player)
    {
        thisPlayer = player;
    }

    void Start() {
        mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;
    }

    private bool ShowGraphic(Collider other) {
        if (thisPlayer == null)
            return false; // happens on ohter player

        return (other.tag == "Player" && other.gameObject != thisPlayer.gameObject);
    }

    void OnTriggerEnter(Collider other) {
        if (!ShowGraphic(other))
            return;

        mesh.enabled = true;
    }

    void OnTriggerExit(Collider other) {
        if (!ShowGraphic(other))
            return;
        mesh.enabled = false;
    }
}