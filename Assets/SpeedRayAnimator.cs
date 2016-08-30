using UnityEngine;
using System.Collections;

public class SpeedRayAnimator : MonoBehaviour {
    private float animationCurrent;
    private float animationSpeed = 0.1f;
    private Material mat;

    //use for direction and speed
    public float rotationSpeed = 1;

    void Start() {
        mat = GetComponent<MeshRenderer>().material;
    }

    void Update() {
        // rotate cone (model)
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        animationSpeed = UniverseTransformer.Instance.CurrentSpeed;
        print("SPEED: " + animationSpeed);

        // animate texture
        if (animationCurrent < 1) {
            animationCurrent += Time.deltaTime * animationSpeed;
        }
        else {
            animationCurrent = 0;
        }
        mat.mainTextureOffset = Vector2.up * animationCurrent;
    }
}