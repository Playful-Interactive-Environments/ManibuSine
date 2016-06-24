using UnityEngine;
using System.Collections;

public class UniverseTransformer : MonoBehaviour {

    private static UniverseTransformer instance;
    public static UniverseTransformer Instance { get { return instance; } }

    private Transform targetTransfrom;
    private Transform targetRotator;
    public Transform shipTransform;

    private Rigidbody targetBody;
    private Rigidbody targetRotatorBody;

    // TODO: check and ajdust lerp speed - could reduce jittering
    public float lerpSpeed = 0.1f;
    private Quaternion oldRot = new Quaternion();
    private Material sky;

    void Awake()
    {
        instance = this;
    }

    void Start() {
        sky = RenderSettings.skybox;
    }

    public Transform GetTargetTransform() {
        return targetTransfrom;
    }

    // Use this for initialization
    public void SetTargetTransform(Transform target) {
        targetTransfrom = target;
        targetBody = target.GetComponent<Rigidbody>();
    }
    // Use this for initialization
    public void SetTargetRotator(Transform target)
    {
        targetRotator = target;
        targetRotatorBody = target.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        if (targetTransfrom == null && targetRotator == null)
            return;

        transform.position = Vector3.Lerp(transform.position, targetTransfrom.position, lerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTransfrom.rotation, lerpSpeed * Time.deltaTime);

        RotateSkyBox(transform.rotation.eulerAngles.y);

    }

    private void RotateSkyBox(float rot) {
        sky.SetFloat("_Rotation", -rot);
    }

    public void MoveForward(float s)
    {
        //targetTransfrom.Translate(shipTransform.right * -s * Time.deltaTime, Space.World);
        targetBody.AddForce(shipTransform.forward * s * Time.fixedDeltaTime);
    }

    public void RotateUniverse(float a)
    {
        //targetRotatorBody.AddTorque(Vector3.up * a * Time.deltaTime);
        targetTransfrom.RotateAround(shipTransform.position, Vector3.up, a * Time.fixedDeltaTime);
        
    }
}