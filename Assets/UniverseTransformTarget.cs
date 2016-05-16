using UnityEngine;
using System.Collections;

public class UniverseTransformTarget : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UniverseTransformer.Instance.transformTarget = this.transform;
	}
}
