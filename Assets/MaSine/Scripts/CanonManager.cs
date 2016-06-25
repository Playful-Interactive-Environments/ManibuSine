using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public delegate void CanonDelegateSimple();
public delegate void CanonDelegateTransform(CanonManager canonManager);
public delegate void CanonDelegateID(uint id);

public class CanonManager : NetworkBehaviour
{
    public int id;
    public float rotation;
    public static CanonDelegateTransform GotTarget, EnteredCannon, ExitCannon;
    public static CanonDelegateSimple LostTarget;

    public CannonPivot cannonPivot;

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

    private bool isTargetingEnabled = true;

    void Start()
    {
        canon = cannonPivot.GetComponentInChildren<Canon>();
        asource = GetComponent<AudioSource>();
        transform.rotation = Quaternion.Euler(0, rotation, 0);
        CannonPivot.OutOfRange += OnOutOfRange;
        CannonPivot.InRange += OnInRange;

    }

    private void OnInRange(uint id)
    {
        isTargetingEnabled = true;
    }

    private void OnOutOfRange(uint id)
    {
        isTargetingEnabled = false;
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

        gunnerHead.target = null;
    }

    void Shoot()
    {
        if (isServer)
            return;
        if (shootCooldown <= 0.0f && isTargetingEnabled)
        {
            networkPlayer.CmdShoot(this.netId.Value);
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
                    if (isTargetingEnabled)
                    {
                        // start targeting sound
                        asource.clip = targetingClip;
                        asource.pitch = 1.0f;
                        asource.Play();
                    }
                    if (GotTarget != null)
                            GotTarget(this);
                    
                    
                }

                if(isTargetingEnabled) targetedTime += Time.deltaTime / targetingSpeed;

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
            new Vector3(gunner.transform.position.x, 
                        cannonPivot.transform.position.y, 
                        cannonPivot.transform.position.z),
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