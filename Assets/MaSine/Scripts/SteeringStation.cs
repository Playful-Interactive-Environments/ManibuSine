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

    private float speedMulti = 50;
    private float angleMulti = 0.1f;

    private NetworkPlayer networkPlayer;

    // Use this for initialization
    void Start () {
        mRenderer = GetComponent<MeshRenderer>();
        originalColor = mRenderer.material.color;
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
        uiArrowLength = (uiDistance - trigger.transform.lossyScale.x / 2) / (this.transform.lossyScale.x / 2 - trigger.transform.lossyScale.x / 2);

        //STEERING VARIABLES
        float distance = navigator.position.x - transform.position.x;
        if (distance < trigger.transform.lossyScale.x / 2) 
        {
            speedInput = 0.0f;
            return;
        }
            
        speedInput = (distance - trigger.transform.lossyScale.x / 2) / (this.transform.lossyScale.x / 2 - trigger.transform.lossyScale.x / 2);
        
    }

    // PlayerAssigned Msg sent in cannon trigger
    void MsgPlayerAssigned(Transform navigator)
    {
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

        if (this.navigator != null)
            return;

        this.navigator = navigator;
        networkPlayer = navigator.GetComponent<NetworkPlayer>();
    }

    // PlayerGone Msg sent in cannon trigger
    void PlayerLeftStation(Transform leavingPlayer)
    {
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

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "NetworkPlayer")
        {
            PlayerLeftStation(other.transform);
        }
    }
}
