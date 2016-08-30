using UnityEngine;
using System.Collections;

public class SpeedRayAnimator : MonoBehaviour {
    private float animationCurrent;
    private float animationSpeed = 1.0f;


    // TODO: do this for all materials (2 children)
    private Material mat;

    void Start () {
        mat = GetComponentInChildren<MeshRenderer>().material;
    }
	
	void Update () {
        if (animationCurrent < 1) {
            animationCurrent += Time.deltaTime * animationSpeed;
        }
        else {
            animationCurrent = 0;
        }

        mat.mainTextureOffset = Vector2.up * animationCurrent;
    }
}
