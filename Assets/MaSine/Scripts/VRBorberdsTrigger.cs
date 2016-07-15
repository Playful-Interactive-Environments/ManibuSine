using UnityEngine;
using System.Collections;

public class VRBorberdsTrigger : MonoBehaviour {

    public static void AssignPlayer(NetworkPlayer player) {
        VRBorberdsTrigger[] borders = FindObjectsOfType<VRBorberdsTrigger>();
        foreach (VRBorberdsTrigger item in borders) {
            item.SetPlayer(player);
        }
    }

    private void SetPlayer(NetworkPlayer player) {
        this.player = player.gameObject;
    }

    private GameObject player;
    public GameObject vrBorderParent;
    private MeshRenderer[] meshes;
	// Use this for initialization
	void Start () {
        meshes = vrBorderParent.GetComponentsInChildren<MeshRenderer>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player)
            return;
        foreach(MeshRenderer mesh in meshes)
        {
            mesh.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject != player)
            return;
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.enabled = false;
        }
    }
}
