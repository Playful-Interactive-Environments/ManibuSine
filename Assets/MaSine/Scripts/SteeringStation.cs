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
    private float distance;
    public float angleInput;
    public float uiSpeedScale;

    public float playerDropOutDelay = 3.0f;
    private IEnumerator dropPlayerCoroutine;

    public Transform navigator;

    public UniverseTransformer universeTransformer;

    private float speedMulti = 80;
    private float angleMulti = 0.1f;

    private NetworkPlayer networkPlayer;
    public NetworkPlayer NetworkPlayer
    {
        get { return networkPlayer; }
    }

    private AudioSource source;
    private AudioFader audioFader;

    // Use this for initialization
    void Start () {
        mRenderer = GetComponentInChildren<MeshRenderer>();
        originalColor = mRenderer.material.color;
        source = GetComponent<AudioSource>();
        audioFader = GetComponent<AudioFader>();
    }

    // Update is called once per frame
    void FixedUpdate () {

        if (navigator != null)
        {
            CalculateSpeedInput();
            CalculateAngleInput();


            //if (distance < 0)
            //    return;

            if (isServer)
                return;

            // Send Cmd to server to move ship
            networkPlayer.CmdMoveShipForward(speedInput * speedMulti);
            networkPlayer.CmdRotateShipCW(angleInput * angleMulti);
        }
        

        if(navigator != null)
        {  
            
        }
	}

    private void CalculateAngleInput()
    {
        float uiAngleDistance = (transform.InverseTransformPoint(navigator.position)).z;
        float clampedPositionZ = Mathf.Clamp(uiAngleDistance / (this.transform.lossyScale.z / 2.0f), -1, 1);

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
        distance = (transform.InverseTransformPoint(navigator.position)).x;

        uiSpeedScale = Mathf.Clamp01(distance / (this.transform.lossyScale.x / 2.0f));

        //STEERING VARIABLES
        

        speedInput = Mathf.Clamp01(distance / (this.transform.lossyScale.x / 2.0f));

        // Commented out to fly backwards
        //if (distance < 0.001f) 
        //{
        //    speedInput = 0.0f;
        //    //return;
        //}
        print(speedInput);
        source.pitch = 0.5f + speedInput / 2.0f;
        source.volume = Mathf.Abs(speedInput);// / 2.0f;
    }

    // PlayerAssigned Msg sent in cannon trigger
    void MsgPlayerAssigned(Transform navigator)
    {
        if (this.navigator != null)
            return;

        this.navigator = navigator;

        source.Play();
        mRenderer.material.color = assignedColor;

        networkPlayer = navigator.GetComponent<NetworkPlayer>();

        if (EnteredSteering != null)
            EnteredSteering(this);

        if (!isServer)
            UI_HeadUpText.DisplayText(  UI_HeadUpText.DisplayArea.TopRight, 
                                        GameColor.Neutral, 
                                        UI_HeadUpText.TextSize.small,
                                        "enter cockpit",
                                        2);
    }

    // PlayerGone Msg sent in cannon trigger
    void PlayerLeftStation(Transform leavingPlayer)
    {
        audioFader.FadeOut(0.333f);

        if (leavingPlayer != this.navigator)
            return;

        navigator = null;

        mRenderer.material.color = originalColor;

        if (ExitedSteering != null)
            ExitedSteering(this);

        if (!isServer)
            UI_HeadUpText.DisplayText(  UI_HeadUpText.DisplayArea.TopRight,
                                        GameColor.Neutral,
                                        UI_HeadUpText.TextSize.small,
                                        "exit cockpit",
                                        2);
    }

    void MsgPlayerGone(Transform leavingPlayer)
    {
        PlayerLeftStation(leavingPlayer);
    }
}
