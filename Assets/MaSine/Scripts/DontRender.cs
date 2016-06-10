using UnityEngine;
using System.Collections;

public class DontRender : MonoBehaviour {
	// Use this for initialization
	void Start () {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null) // TODO maybe delete component
            mr.enabled = false;
	}
}