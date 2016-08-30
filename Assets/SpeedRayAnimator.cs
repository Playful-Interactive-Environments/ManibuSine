using UnityEngine;
using System.Collections;

public class SpeedRayAnimator : MonoBehaviour {
    private float maxAnimationSpeed = 0.3f;
    private float maxOpacity = 0.1f;

    private float animationCurrent;
    private float animationSpeed = 0.6f;
    private Material mat;

    //use for direction and speed
    public float rotationSpeed = 1;

    void Start () {
        mat = GetComponent<MeshRenderer>().material;
    }
	
	void Update () {
        // no animation if there is no steering station
        if (SteeringStation.Instance == null) {
            animationSpeed = 0;
            return;
        }

        // rotate cone (model)
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // calculate animation speed. lerp to animation speed - smooth
        animationSpeed = Mathf.Lerp(animationSpeed, SteeringStation.Instance.uiSpeedScale * maxAnimationSpeed, Time.deltaTime);

        // rotate rays. fake some "inertia"
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(-90, 0, SteeringStation.Instance.angleInput * 1.1f), Time.deltaTime * 0.1f);



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