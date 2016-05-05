using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CanonManager : NetworkBehaviour
{
    public delegate void CanonDelegateTransform(CanonManager canonManager);
    public CanonDelegateTransform GotTarget;
    public delegate void CanonDelegateSimple();
    public CanonDelegateSimple LostTarget;

    public GameObject canonPivot;

    public Transform gunner;
    private Head gunnerHead;
    public Transform TargetTransform
    {
        get { return gunnerHead.target; }
    }
    private Canon canon;

    NetworkPlayer networkPlayer;

    private float rotationSpeed = 3;
    private float translationSpeed = 5;

    private float targetedTime = 0.0f;
    public float TargetedTime { get { return targetedTime; } }
    private float targetingSpeed = 1.0f;
    private Quaternion startQuat;

    private float shootSpeed = 1.0f;
    private float shootCooldown = 0.0f;


    void Start()
    {
        canon = canonPivot.GetComponentInChildren<Canon>();
        InvokeRepeating("RegisterAtNetworDataManager", 0.5f, 0.5f);
    }

    void RegisterAtNetworDataManager()
    {
        NetworkPlayer nwp = FindObjectOfType<NetworkPlayer>();
        if (nwp != null && nwp.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            networkPlayer = nwp;
        }

        if (networkPlayer != null)
            CancelInvoke("RegisterAtNetworDataManager");
    }


    // PlayerAssigned Msg sent in cannon trigger
    void PlayerAssigned(Transform gunner)
    {
        this.gunner = gunner;
        this.gunnerHead = gunner.GetComponentInChildren<Head>();
    }

    // PlayerGone Msg sent in cannon trigger
    void PlayerGone()
    {
        gunner = null;
        gunnerHead = null;
    }

    void Shoot()
    {
        if (shootCooldown <= 0.0f)
        {
            networkPlayer.CmdShoot();
            shootCooldown = shootSpeed;
        }
            
    }

    void Update()
    {
        if (shootCooldown > 0.0f)
        {
            shootCooldown -= Time.deltaTime;
        }
        
        if (ServerManager.Instance.isServer)
        {
            NetworkServer.Spawn(gameObject);
        }
        if (gunner != null)
        {

            // rotate canon
            if (gunnerHead.target != null)
            {
                // got new target
                if (targetedTime == 0)
                {
                    startQuat = canonPivot.transform.rotation;
                    if (GotTarget != null)
                        GotTarget(this);
                }

                targetedTime += Time.deltaTime/targetingSpeed;

                Quaternion targetRot = Quaternion.LookRotation(gunnerHead.target.transform.position - canonPivot.transform.position);
                canonPivot.transform.rotation = Quaternion.Lerp(startQuat, targetRot, targetedTime);

                if (targetedTime >= 1.0f)
                {
                    Shoot();
                }
            }
            else
            {
                canonPivot.transform.rotation =
                Quaternion.Lerp(canonPivot.transform.rotation,
                gunnerHead.transform.rotation,
                rotationSpeed * Time.deltaTime);

                targetedTime = 0.0f;
            }
            


            // move canon
            canonPivot.transform.position =
            Vector3.Lerp(canonPivot.transform.position,
            new Vector3(canonPivot.transform.position.x, canonPivot.transform.position.y, gunner.transform.position.z),
        translationSpeed * Time.deltaTime);
            if (gunner.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
        }
    }
}
