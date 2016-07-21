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
    public static CanonDelegateID LostTarget;

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

    private bool cannonWithinLimits = true;

    private bool hasTarget = false;

    void Start()
    {
        canon = cannonPivot.GetComponentInChildren<Canon>();
        asource = GetComponent<AudioSource>();
        //transform.rotation = Quaternion.Euler(0, rotation, 0);
        CannonPivot.OutOfRange += OnWithinLimits;
        CannonPivot.InRange += OnOverLimits;

    }

    private void OnOverLimits(uint id)
    {
        if (this.netId.Value != id)
            return;
        cannonWithinLimits = true;
    }

    private void OnWithinLimits(uint id)
    {
        if (this.netId.Value != id)
            return;
        cannonWithinLimits = false;
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

        targetedTime = 0.0f;

        this.gunner = gunner;
        this.gunnerHead = gunner.GetComponentInChildren<Head>();

        this.networkPlayer = gunner.GetComponent<NetworkPlayer>();

        if (EnteredCannon != null)
            EnteredCannon(this);

        // HUD
        if (!isServer)
            UI_HeadUpText.DisplayText(UI_HeadUpText.DisplayArea.TopRight,
                                       GameColor.Neutral,
                                       UI_HeadUpText.TextSize.small,
                                       "enter cannon",
                                       2);
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

        if (gunnerHead != null) gunnerHead.target = null;

        // HUD
        if (!isServer)
            UI_HeadUpText.DisplayText(UI_HeadUpText.DisplayArea.TopRight,
                                       GameColor.Neutral,
                                       UI_HeadUpText.TextSize.small,
                                       "exit cannon",
                                       2);
    }

    void Update()
    {
        CannonHandling();
    }

    private void Shoot()
    {
        if (isServer)
            return;
        if (shootCooldown <= 0.0f && cannonWithinLimits)
        {
            networkPlayer.CmdShoot(this.netId.Value);
            shootCooldown = shootSpeed;
            canon.ShootEffects();
        }
    }

    private void CannonHandling()
    {
        if (gunner == null)
            return;
        // execute only on local player
        ClientOnlyCannonConrtol();

        // move canon (server and client)
        MoveCannon();

        // rotate cannon (server and client)
        RotateCannon();
    }

    private void ClientOnlyCannonConrtol()
    {
        if (!isClient)
            return;
        if (shootCooldown > 0.0f)
            shootCooldown -= Time.deltaTime;

        // has target
        if (gunnerHead.target != null)
        {
            if (cannonWithinLimits)
            {
                // start targeting
                ClientStartTargeting();

                // targeting process is running
                //if (cannonWithinLimits)
                targetedTime += Time.deltaTime / targetingSpeed;

                // shoot when time passed
                if (targetedTime >= targetingDuration)
                    Shoot();
                else // pitch sound
                    asource.pitch = (targetedTime + 0.8f) / 2 + targetingDuration;
            }
        }
        else
        {
            // reset targeting time
            targetedTime = 0.0f;

            // lost target
            if (hasTarget)
            {
                hasTarget = false;
                if (LostTarget != null)
                    LostTarget(this.netId.Value);
            }

            // stop targeting sound
            if (asource.isPlaying)
                asource.Stop();
        }
    }

    private void ClientStartTargeting()
    {
        if (targetedTime == 0)
        {
            startQuat = cannonPivot.transform.rotation;

            hasTarget = true;
            // start targeting sound
            asource.clip = targetingClip;
            asource.pitch = 1.0f;
            asource.Play();
            if (GotTarget != null)
                GotTarget(this);
        }
    }

    private void MoveCannon()
    {
        cannonPivot.transform.localPosition = Vector3.Lerp(
                    cannonPivot.transform.localPosition,
                    new Vector3(transform.InverseTransformPoint(gunner.transform.position).x, cannonPivot.transform.localPosition.y, cannonPivot.transform.localPosition.z),
                    translationSpeed * Time.deltaTime);
    }

    private void RotateCannon()
    {
        // apply rotation to cannon with target (server and client)
        if (gunnerHead.target != null && cannonWithinLimits)
        {
            Quaternion targetRot = Quaternion.LookRotation(gunnerHead.aimPoint - cannonPivot.transform.position);
            cannonPivot.transform.rotation = Quaternion.Lerp(startQuat, targetRot, targetedTime);
        }
        else // apply  rotation without target
        {
            if (isClient) // CLIENT
                cannonPivot.transform.rotation = Quaternion.Lerp(cannonPivot.transform.rotation, gunnerHead.transform.rotation, rotationSpeed * Time.deltaTime);
            else if (isServer) // SERVER
            {// lerping on server causes targeting delay
                cannonPivot.transform.rotation = gunnerHead.transform.rotation;
            }
        }
    }



    // [deprecated] - kept for documentation
    //void OldCanonUpdate()
    //{
    //    if (shootCooldown > 0.0f)
    //    {
    //        shootCooldown -= Time.deltaTime;
    //    }
    //    if (gunner != null)
    //    {

    //        // rotate canon
    //        if (gunnerHead.target != null)
    //        {

    //            if (targetedTime == 0)
    //            {

    //                startQuat = cannonPivot.transform.rotation;
    //                if (cannonWithinLimits)
    //                {
    //                    // start targeting sound
    //                    asource.clip = targetingClip;
    //                    asource.pitch = 1.0f;
    //                    asource.Play();
    //                    hasTarget = true;
    //                }
    //                if (GotTarget != null)
    //                    GotTarget(this);


    //            }

    //            if (cannonWithinLimits) targetedTime += Time.deltaTime / targetingSpeed;
    //            //else targetedTime = 0.1f;

    //            Quaternion targetRot = Quaternion.LookRotation(gunnerHead.aimPoint - cannonPivot.transform.position);
    //            cannonPivot.transform.rotation = Quaternion.Lerp(startQuat, targetRot, targetedTime);


    //            //Debugray to show where the canon is aiming
    //            //Debug.DrawRay(canon.transform.position, (gunnerHead.aimPoint - canon.transform.position), Color.red);

    //            if (targetedTime >= targetingDuration)
    //            {
    //                Shoot();
    //            }
    //            else
    //            {
    //                // pitch sound
    //                asource.pitch = (targetedTime + 0.8f) / 2 + targetingDuration;
    //            }
    //        }
    //        else
    //        {
    //            if (isServer)
    //            {// lerping on server causes targeting delay
    //                cannonPivot.transform.rotation = gunnerHead.transform.rotation;
    //            }
    //            else
    //            {
    //                cannonPivot.transform.rotation = Quaternion.Lerp(cannonPivot.transform.rotation, gunnerHead.transform.rotation, rotationSpeed * Time.deltaTime);
    //            }


    //            // stop targeting sound
    //            if (asource.isPlaying)
    //                asource.Stop();

    //            targetedTime = 0.0f;

    //            if (hasTarget)
    //            {
    //                hasTarget = false;
    //                if (LostTarget != null)
    //                    LostTarget(this.netId.Value);
    //            }
    //        }

    //        // move canon
    //        cannonPivot.transform.position =
    //        Vector3.Lerp(cannonPivot.transform.position,
    //        new Vector3(gunner.transform.position.x,
    //                    cannonPivot.transform.position.y,
    //                    cannonPivot.transform.position.z),
    //    translationSpeed * Time.deltaTime);
    //    }
    //}
}