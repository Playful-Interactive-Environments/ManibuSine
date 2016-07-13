using UnityEngine;
using System.Collections;

public class VRBorberdsTrigger : MonoBehaviour {

    public GameObject vrBorderParent;
    private MeshRenderer[] meshes;
	// Use this for initialization
	void Start () {
        meshes = vrBorderParent.GetComponentsInChildren<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        foreach(MeshRenderer mesh in meshes)
        {
            mesh.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.enabled = false;
        }
    }
}
