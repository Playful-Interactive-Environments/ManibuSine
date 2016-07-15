using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class SteeringStation : NetworkBehaviour {

    public delegate void SteeringDelegateTransform(SteeringStation steeringStation);
    public SteeringDelegateTransform  EnteredSteering, ExitedSteering;


    private MeshRenderer mRenderer;
    private Color originalColor;
    public Color assignedColor;

    private float speedInput;
    public float angleInput;
    public float uiSpeedScale;

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
        float uiAngleDistance = navigator.position.z - transform.position.z;
        float clampedPositionZ = Mathf.Clamp(uiAngleDistance / (this.transform.lossyScale.z / 2.0f), -1, 1);
        print(clampedPositionZ);

        angleInput = clampedPositionZ * 90;
        //Debug.DrawRay(this.transform.position, 
        //    new Vector3(Mathf.Cos(Mathf.Atan2(y, x)), 
        //    0,  
        //    Mathf.Sin(Mathf.Atan2(y, x))) * 10000, Color.blue);
    }

    private void CalculateSpeedInput()
    {
        PlayerAssignmentTrigger trigger = GetComponentInChildren<PlayerAssignmentTrigger>();
        //UI VARIABLES
        float uiDistance = navigator.position.x - transform.position.x;
        uiSpeedScale = Mathf.Clamp01(uiDistance / (this.transform.lossyScale.x / 2.0f));

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
        if (this.navigator != null)
            return;

        source.Play();
        mRenderer.material.color = assignedColor;

        if (EnteredSteering != null)
            EnteredSteering(this);
        
        //if (!isServer)
        //    UI_HeadUpText.ShowTextOnHeadUp("Enter Cockpit", 2);

        this.navigator = navigator;
        networkPlayer = navigator.GetComponent<NetworkPlayer>();
    }

    // PlayerGone Msg sent in cannon trigger
    void PlayerLeftStation(Transform leavingPlayer)
    {
        source.Stop();
        if (leavingPlayer != this.navigator)
            return;

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
