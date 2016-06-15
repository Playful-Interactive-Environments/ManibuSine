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

    private float targetingDuration = 1.0f;
    private float targetedTime = 0.0f;
    public float TargetedTime { get { return targetedTime; } }
    private float targetingSpeed = 1.0f;
    private Quaternion startQuat;

    private float shootSpeed = 1.0f;
    private float shootCooldown = 0.0f;
    private AudioSource asource;
    public AudioClip targetingClip;

    void Start()
    {
        canon = cannonPivot.GetComponentInChildren<Canon>();
        asource = GetComponent<AudioSource>();
    }

    public bool IsGunnerLocalPlayer()
    {
        if (networkPlayer != null)
            return networkPlayer.isLocalPlayer;

        return false;
    }

    // PlayerAssigned Msg sent in cannon trigger
    void MsgPlayerAssigned(Transform gunner)
    {
        if (this.gunner != null)
            return;

        this.gunner = gunner;
        this.gunnerHead = gunner.GetComponentInChildren<Head>();

        this.networkPlayer = gunner.GetComponent<NetworkPlayer>();

        if (EnteredCannon != null)
            EnteredCannon(this);
    }

    // PlayerGone Msg sent in cannon trigger
    void MsgPlayerGone(Transform leavingGunner)
    {
        if (gunner == null)
            return;

        if (leavingGunner != gunner)
            return;

        gunner = null;
        gunnerHead = null;

        if (ExitCannon != null)
            ExitCannon(this);
    }

    void Shoot()
    {
        if (isServer)
            return;
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
        if (gunner != null)
        {

            // rotate canon
            if (gunnerHead.target != null)
            {
                if (targetedTime == 0)
                {
                    startQuat = cannonPivot.transform.rotation;

                    // start targeting sound
                    asource.clip = targetingClip;
                    asource.pitch = 1.0f;
                    asource.Play();

                    if (GotTarget != null)
                        GotTarget(this);
                }

                targetedTime += Time.deltaTime / targetingSpeed;

                Quaternion targetRot = Quaternion.LookRotation(gunnerHead.aimPoint - cannonPivot.transform.position);
                cannonPivot.transform.rotation = Quaternion.Lerp(startQuat, targetRot, targetedTime);

                //Debugray to show where the canon is aiming
                Debug.DrawRay(canon.transform.position, (gunnerHead.aimPoint - canon.transform.position), Color.red);

                if (targetedTime >= targetingDuration)
                {
                    Shoot();
                }
                else
                {
                    // pitch sound
                    asource.pitch = (targetedTime + 0.8f) / 2 + targetingDuration;
                }
            }
            else
            {
                cannonPivot.transform.rotation =
                Quaternion.Lerp(cannonPivot.transform.rotation,
                gunnerHead.transform.rotation,
                rotationSpeed * Time.deltaTime);

                // stop targeting sound
                if (asource.isPlaying)
                    asource.Stop();

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