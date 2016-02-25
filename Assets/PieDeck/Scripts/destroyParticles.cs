using UnityEngine;
using System.Collections;

public class destroyParticles : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine("destroyMe");
	}
	IEnumerator destroyMe(){
		yield return new WaitForSeconds (4.0f);
		Destroy (gameObject);
	}
}
