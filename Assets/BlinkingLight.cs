using UnityEngine;
using System.Collections;

public class BlinkingLight : MonoBehaviour {
    private SpriteRenderer sr;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        InvokeRepeating("Blink", 1.7f, 1.7f);
	}

    void Blink() {
        sr.enabled = !sr.enabled;
    }
	
}
