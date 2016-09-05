using UnityEngine;
using System.Collections;

public class SpeedRayAnimator : MonoBehaviour {
    private float maxAnimationSpeed = 0.3f;
    private float maxOpacity = 0.03f;

    private float currentSpeed = 0;
    private float animationCurrent;
    
    private Material mat;
    private Color rayColor;

    //use for direction and speed
    public float rotationSpeed = 1;

    void Start () {
        mat = GetComponent<MeshRenderer>().material;
        rayColor = mat.GetColor("_TintColor");
    }
    void Update() {
        // no animation if there is no steering station
        // TODO: check if game over works (|| ...enabled == false)
        if (SteeringStation.Instance == null || SteeringStation.Instance.enabled == false) {
            currentSpeed = 0;
            rayColor.a = 0;
            mat.SetColor("_TintColor", rayColor);
            return;
        }
        currentSpeed = Mathf.Lerp(currentSpeed, SteeringStation.Instance.uiSpeedScale, Time.deltaTime);

        SetMaterialAlpha();
        AnimateTexture();
        TransformObject();
    }

    private void SetMaterialAlpha() {
        rayColor.a = currentSpeed * maxOpacity;
        mat.SetColor("_TintColor", rayColor);
    }
    private void AnimateTexture() {
        // apply animation on texture
        if (animationCurrent < 1)
            animationCurrent += Time.deltaTime * currentSpeed * maxAnimationSpeed;
        else
            animationCurrent = 0;

        mat.mainTextureOffset = Vector2.up * animationCurrent;
    }
    private void TransformObject() {
        // rotate/roll cone(mesh) slowly to provide variety
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        // rotate rays according to flight direction - fake some "inertia"
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(-90, 0, -SteeringStation.Instance.angleInput * 0.16f), Time.deltaTime * 0.1f);
    }
}