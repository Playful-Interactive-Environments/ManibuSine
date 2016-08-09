using UnityEngine;
using System.Collections;

public class DestroyInTime : MonoBehaviour {

    public float secondsToDestruction;

	// Use this for initialization
	void Start () {
        Invoke("DoDestroy", secondsToDestruction);
	}
	
	void DoDestroy()
    {
        Destroy(this.gameObject);
    }
}
