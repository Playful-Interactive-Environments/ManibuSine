using UnityEngine;
using System.Collections;

public class VR_CylinderBorder : MonoBehaviour {

    private MeshRenderer mesh;
    private NetworkPlayer thisPlayer;


    void Start() {
        mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;
    }

    private bool IsOwnGraphic(Collider other) {
        return (other.tag != "Player" || !thisPlayer.isLocalPlayer);
    }

    void OnTriggerEnter(Collider other) {
        if (!IsOwnGraphic(other))
            return;
        mesh.enabled = true;
    }

    void OnTriggerExit(Collider other) {
        if (!IsOwnGraphic(other))
            return;
        mesh.enabled = false;
    }
}