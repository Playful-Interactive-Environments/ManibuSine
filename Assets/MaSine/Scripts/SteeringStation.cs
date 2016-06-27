using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class SteeringStation : NetworkBehaviour {

    public delegate void SteeringDelegateTransform(SteeringStation steeringStation);
    public SteeringDelegateTransform  EnteredSteering, ExitedSteering, StepedOutSteering;


    private MeshRenderer mRenderer;
    private Color originalColor;
    public Color assignedColor;

    private float speedInput;
    public float angleInput;
    public float uiArrowLength;

    public float playerDropOutDelay = 3.0f;
    private IEnumerator dropPlayerCoroutine;

    public Transform navigator;

    public UniverseTransformer universeTransformer;

    private float speedMulti = 80;
    private float angleMulti = 0.1f;

    private NetworkPlayer networkPlayer;

    private AudioSource source;

    // Use this for initialization
    void Start () {
        mRenderer = GetComponentInChildren<MeshRenderer>();
        originalColor = mRenderer.material.color;
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (isServer)
            return;
        if(navigator != null)
        {
            CalculateSpeedInput();
            CalculateAngleInput();
            if (angleInput > 90 || angleInput < -90)
                return; 
                
            networkPlayer.CmdMoveShipForward(speedInput * speedMulti);
            networkPlayer.CmdRotateShipCW(angleInput * angleMulti);
        }
	}

    private void CalculateAngleInput()
    {
        float x = navigator.position.x - transform.position.x;
        float y = navigator.position.z - transform.position.z;

        angleInput = Mathf.Rad2Deg * Mathf.Atan2(y, x);
        //Debug.DrawRay(this.transform.position, 
        //    new Vector3(Mathf.Cos(Mathf.Atan2(y, x)), 
        //    0,  
        //    Mathf.Sin(Mathf.Atan2(y, x))) * 10000, Color.blue);
    }

    private void CalculateSpeedInput()
    {
        PlayerAssignmentTrigger trigger = GetComponentInChildren<PlayerAssignmentTrigger>();
        //UI VARIABLES
        float uiDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(navigator.position.x, navigator.position.z));
        uiArrowLength = uiDistance / (this.transform.lossyScale.x / 2.0f);

        //STEERING VARIABLES
        float distance = navigator.position.x - transform.position.x;

        speedInput = Mathf.Clamp01(distance / (this.transform.lossyScale.x / 2.0f));

        if (distance < 0.001f) 
        {
            speedInput = 0.0f;
            //return;
        }

        source.pitch = 0.5f + speedInput / 2.0f;
        source.volume = speedInput;// / 2.0f;
    }

    // PlayerAssigned Msg sent in cannon trigger
    void MsgPlayerAssigned(Transform navigator)
    {
        source.Play();
        if (navigator == this.navigator)
        {
            if (dropPlayerCoroutine != null)
            {
                StopCoroutine(dropPlayerCoroutine);
            }
        }

        mRenderer.material.color = assignedColor;

        if (EnteredSteering != null)
            EnteredSteering(this);

        //if (!isServer)
        //    UI_HeadUpText.ShowTextOnHeadUp("Enter Cockpit", 2);

        if (this.navigator != null)
            return;

        this.navigator = navigator;
        networkPlayer = navigator.GetComponent<NetworkPlayer>();
    }

    // PlayerGone Msg sent in cannon trigger
    void PlayerLeftStation(Transform leavingPlayer)
    {
        source.Stop();
        if (leavingPlayer != this.navigator)
            return;

        dropPlayerCoroutine = UnassignPlayer();
        StartCoroutine(dropPlayerCoroutine);

        if (StepedOutSteering != null)
            StepedOutSteering(this);
    }

    IEnumerator UnassignPlayer()
    {
        yield return new WaitForSeconds(playerDropOutDelay);

        if (this.navigator == null)
            yield break;

        navigator = null;

        mRenderer.material.color = originalColor;

        if (ExitedSteering != null)
            ExitedSteering(this);
    }

    void MsgPlayerGone(Transform leavingPlayer)
    {
        PlayerLeftStation(leavingPlayer);
    }
}
