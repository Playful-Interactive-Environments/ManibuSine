using UnityEngine;
using System.Collections;

public class Head : MonoBehaviour
{
    public Transform target;
    public Transform pickUp;

    public Vector3 aimPoint;
    public LayerMask cannonMask;
    public LayerMask steeringMask;
    private LayerMask mask;
    //private AudioManager audioManager;
    //private AudioSource asource;
    NetworkPlayer player;

    

    // Use this for initialization
    void Start()
    {
        //audioManager = AudioManager.Instance;
        player = GetComponentInParent<NetworkPlayer>();

        CanonManager.EnteredCannon += OnEnterCannon;
        //CanonManager.ExitCannon += OnExitCannon;

        //SteeringStation steering = FindObjectOfType<SteeringStation>();
        SteeringStation.EnteredSteering += OnEnteredSteering;
        SteeringStation.ExitedSteering += OnExitSteering;
    }

    private void OnEnterCannon(CanonManager canonManager)
    {
        mask = cannonMask;
    }

    // check if either is local player or if no render client
    private bool CheckEnteringPlayer(SteeringStation steeringStation) {
        return (steeringStation.NetworkPlayer.isLocalPlayer || (!ServerManager.Instance.isServer && ClientChooser.Instance.clientType != ClientChooser.ClientType.VRClient));
    }

    //private void OnExitCannon(CanonManager canonManager)
    //{
        
    //}
    private void OnEnteredSteering(SteeringStation steeringStation)
    {
        if (CheckEnteringPlayer(steeringStation))
            mask = steeringMask;
    }
    private void OnExitSteering(SteeringStation steeringStation)
    {
        if (CheckEnteringPlayer(steeringStation))
            mask = cannonMask;
    }

    Ray ray;
    void Update()
    {
        // only network player
        if (player.isServer)//if (!player.isLocalPlayer)
            return;

        ray.origin = transform.position;
        ray.direction = transform.forward;
        RaycastHit hit;

        Physics.Raycast(ray, out hit, 1000, mask);

        // nothin hit
        if (hit.transform == null)
        {
            target = null;
            pickUp = null;
            return;
        }
        // used by cannon station
        aimPoint = hit.point;

        if (hit.transform.tag == "Asteroid")
            target = hit.transform;
        else if (hit.transform.tag == "PickUpCollider")
            pickUp = hit.transform.parent;
        else
        {
            target = null;
            pickUp = null;
        }
    }
}