using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CanonManager : NetworkBehaviour
{
    public delegate void CanonDelegateTransform(CanonManager canonManager);
    public CanonDelegateTransform GotTarget, EnteredCannon, ExitCannon;
    public delegate void CanonDelegateSimple();
    public CanonDelegateSimple LostTarget;

    public GameObject cannonPivot;

    public Transform gunner;
    private Head gunnerHead;
    public Transform TargetTransform
    {
        get
        {
            if (gunnerHead == null)
                return null;
            return gunnerHead.target;
        }
    }
    public Vector3 aimPoint
    {
        get
        {
            return gunnerHead.aimPoint;
        }
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
    private AudioSource asource;
    private AudioManager audioManager;

    void Start()
    {
        canon = cannonPivot.GetComponentInChildren<Canon>();
        audioManager = AudioManager.Instance;
    }


    public bool IsGunnerLocalPlayer()
    {
        if (networkPlayer != null)
            return networkPlayer.isLocalPlayer;

        return false;
    }


    // PlayerAssigned Msg sent in cannon trigger
    void PlayerAssigned(Transform gunner)
    {

        if (gunner != null)
            return;

        this.gunner = gunner;
        this.gunnerHead = gunner.GetComponentInChildren<Head>();

        this.networkPlayer = gunner.GetComponent<NetworkPlayer>();

        if (EnteredCannon != null)
            EnteredCannon(this);
    }

    // PlayerGone Msg sent in cannon trigger
    void PlayerGone()
    {
        gunner = null;
        gunnerHead = null;

        if (ExitCannon != null)
            ExitCannon(this);
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
                if (targetedTime == 0)
                {
                    startQuat = cannonPivot.transform.rotation;
                    if (GotTarget != null)
                        GotTarget(this);
                }

                targetedTime += Time.deltaTime / targetingSpeed;

                Quaternion targetRot = Quaternion.LookRotation(gunnerHead.aimPoint - cannonPivot.transform.position);
                cannonPivot.transform.rotation = Quaternion.Lerp(startQuat, targetRot, targetedTime);

                //Debugray to show where the canon is aiming
                Debug.DrawRay(canon.transform.position, (gunnerHead.aimPoint - canon.transform.position), Color.red);

                //Play sound targeting sound

                if (asource == null)
                {
                    asource = audioManager.PlayClipAt(audioManager.clips[1], audioManager.sources[1], transform.position);
                }

                if (targetedTime >= 1.0f)
                {
                    Shoot();
                }
            }
            else
            {
                cannonPivot.transform.rotation =
                Quaternion.Lerp(cannonPivot.transform.rotation,
                gunnerHead.transform.rotation,
                rotationSpeed * Time.deltaTime);

                targetedTime = 0.0f;
            }



            // move canon
            cannonPivot.transform.position =
            Vector3.Lerp(cannonPivot.transform.position,
            new Vector3(cannonPivot.transform.position.x, cannonPivot.transform.position.y, gunner.transform.position.z),
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