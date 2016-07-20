﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.VR;
using UnityPharus;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    private float headTilt;
    [SyncVar]
    public int levelState = 0;
    [SyncVar]
    public int currentHP;
    
    public GameObject head;

    private GameObject controllingPlayer;
    [SyncVar]
	public Quaternion rotation;
	[SyncVar]
	public Vector3 position;
	private GameObject _vrController;
	private OVRPlayerController _vrControllerScript;
	private Chaperone _chaperoneScript;

	private Vector3 _previousPos;
	private Vector3 _currentPos;
	private float distance;
	private float _timePassed;
	private bool _staticPos;
    [SyncVar]
    public float minMoveDistance;
    [SyncVar]
    public float movementLerpSpeed;

    [SyncVar]
    private bool laserTrackingActivated;

    public GameObject ControllingPlayer
    {
        get
        {
            return controllingPlayer;
        }

        set
        {
            controllingPlayer = value;
            laserTrackingActivated = true;
        }
    }

    void Start () {
        // Initialize movement lerp values
        minMoveDistance = 0.05f;
        movementLerpSpeed = 0.003f;

        if (!isServer)
		{
			_vrController = GameObject.Find("OVRPlayerController");
			_vrControllerScript = _vrController.GetComponent<OVRPlayerController>();
			_chaperoneScript = _vrController.GetComponent<Chaperone>();

            // assign to VR borders
            if (isLocalPlayer)
            {
                // assign to border box
                VRBorberdsTrigger.AssignPlayer(this);
                // assign to cylinder
                VR_CylinderBorder cylinder = GetComponentInChildren<VR_CylinderBorder>();
                if (cylinder != null)
                    cylinder.AssignPlayer(this);

                PickUpRay.PickedItem += OnPickedItem;
            }
        }
        else
        { // SERVER
            laserTrackingActivated = false;

            currentHP = ShipManager.Instance.currentHP;

            ShipCollider.ShipHit += OnShipHit;

            UI_Ship.Instance.SetHP(currentHP);

            RpcSetHP(currentHP);
            print("INIT " + PickUpRay.pickUpsInUpCargo);
            // initiate on restart/client reconnect
            RpcSetItems(PickUpRay.pickUpsInUpCargo);
        }

        // disable renderer of head on local player
        if (isLocalPlayer)
        {
            Head h = GetComponentInChildren<Head>();
            if (h == null)
                return;

            MeshRenderer[] hRenderer = h.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer item in hRenderer)
            {
                item.enabled = false;
            }

            EventTrigger.ShipEnteredEvent += ShipEnteredEvent;
            WaypointLevel wpl = FindObjectOfType<WaypointLevel>();
            if (wpl != null)
            {
                wpl.SyncLevelProgress(levelState);
            }
            transform.FindChild("Body").gameObject.SetActive(false);
        }
	}

    private void OnPickedItem(int picked)
    {
        if (isServer)
            return;

        print("OnPickedItem " + picked);

        CmdSetItems(picked);

        UI_Ship.Instance.SetPickedUp(picked);
    }

    private void OnShipHit(int damage)
    {
        if (!isServer)
            return;

        currentHP -= damage;

        ShipManager.Instance.SetHP(currentHP);
        UI_Ship.Instance.SetHP(currentHP);

        // tell clients
        RpcSetHP(currentHP);
    }

    private void ShipEnteredEvent(IEventTrigger waypoint)
    {
        if (!isLocalPlayer && waypoint != null)
            return;

        CmdSetLevelState(waypoint.GetID());
    }

	// Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if(!laserTrackingActivated)
                LocalPlayerMovement(); // this pos = vrCtrl pos
            //Update Position and Rotation
            if(laserTrackingActivated)
                CalculateVRPos(); // used to update vr camera position
            transform.rotation = _vrController.transform.rotation;

            headTilt = Camera.main.transform.rotation.eulerAngles.x;

            CmdUpdateOrientation(transform.rotation);
            if (!laserTrackingActivated)
                CmdUpdatePosition(transform.position); 

            CmdHeadRotation(headTilt);
        }

        head.transform.rotation = Quaternion.Euler(headTilt, head.transform.rotation.eulerAngles.y, head.transform.rotation.eulerAngles.z);

        if (isServer)
        {
            transform.name = "" + connectionToClient.connectionId;
            GetComponent<CapsuleCollider>().enabled = false;
            if (connectionToClient.connectionId == 1)
            {
                Admin.Instance.PlayerOne = gameObject;
            }
            if (connectionToClient.connectionId == 2)
            {
                Admin.Instance.PlayerTwo = gameObject;

            }
            if (ControllingPlayer != null)
            {
                transform.position = ControllingPlayer.transform.position;
            }
        }
    }
    //----------------------------------------------------------------
    // STUFF WE DO - we are forced to :(
    //----------------------------------------------------------------

    [Command]
    public void CmdSetLevelState(int id)
    {
        levelState = id;
    }

    [Command]
    public void CmdShoot(uint id)
    {
        CanonManager[] cannon = FindObjectsOfType<CanonManager>();
        for(int i = 0; i < cannon.Length; i++)
        {
            if(id == cannon[i].netId.Value)
            {
                cannon[i].GetComponentInChildren<Canon>().Shoot();
                return;
            }
        }
    }
    
    [Command]
    public void CmdDestroyEntity(GameObject obj)
    {
        NetworkServer.Destroy(obj);
    }

    [Command]
    public void CmdMoveShipForward(float speed)
    {
        UniverseTransformer.Instance.MoveForward(speed);
    }

    [Command]
    public void CmdMoveShipBackward(float speed)
    {
        UniverseTransformer.Instance.MoveForward(speed);
    }

    [Command]
    public void CmdRotateShipCW(float rot)
    {
        UniverseTransformer.Instance.RotateUniverse(rot);
    }

    [Command]
    public void CmdRotateShipCCW(float rot)
    {
        UniverseTransformer.Instance.RotateUniverse(rot);
    }

    [Command]
    private void CmdSetItems(int picked)
    {
        // store information on server
        PickUpRay.pickUpsInUpCargo = picked;
        UI_Ship.Instance.SetPickedUp(picked);
    }

    [ClientRpc]
    private void RpcSetItems(int picked) {
        PickUpRay.pickUpsInUpCargo = picked;
        UI_Ship.Instance.SetPickedUp(picked);
    }

    [ClientRpc]
    public void RpcSetHP(int hp)
    {
        ShipManager.Instance.SetHP(hp);
        UI_Ship.Instance.SetHP(hp);
    }


    //----------------------------------------------------------------
    //----------------------------------------------------------------

    void OnDestroy()
    {
        ShipCollider.ShipHit -= OnShipHit;
    }

    void LocalPlayerMovement()
    {
        transform.position = _vrController.transform.position;
    }

	void CalculateVRPos()
	{

        _timePassed += Time.deltaTime;
		if (_timePassed > 3f)
		{
			_previousPos = transform.position;
			_timePassed = 0f;
		}
		_currentPos = transform.position;
		distance = Vector3.Distance(_previousPos, _currentPos);
		if (distance < minMoveDistance)
		{
			_vrController.transform.position = _previousPos;
			_staticPos = true;

		}
		if (distance >= minMoveDistance && !_staticPos)
		{
			_vrController.transform.position = transform.position;
		}
		if (distance >= minMoveDistance && _staticPos)
		{
			_vrController.transform.position = Vector3.Slerp(_previousPos, transform.position, movementLerpSpeed);
			_staticPos = false;
		}
	}

    public void SetMovementLerpSpeed(float val)
    {
        this.movementLerpSpeed = val;
        print("LerpSpeed: " + this.movementLerpSpeed);
    }
    public void SetMinMoveDistance(float val)
    {
        this.minMoveDistance = val;
        print("MinMoveDistance: " + this.minMoveDistance);
    }

    public void ResetOrientation()
	{
		RpcRecalibrateDevice();
	}

	public void ToggleChaperone()
	{
		RpcToggleChaperone();
	}
	#region Network Commands
	[Command]
	void CmdUpdateOrientation(Quaternion rot)
	{
		transform.rotation = rot;
	}

    [Command]
    void CmdUpdatePosition(Vector3 pos)
    {
        transform.position = pos;
    }

    [Command]
    void CmdHeadRotation(float tilt) {
        headTilt = tilt;
    }

	[ClientRpc]
	void RpcRecalibrateDevice()
	{
		if (isLocalPlayer)
		{
			_vrControllerScript.ResetOrientation();
		}

	}
	[ClientRpc]
	void RpcToggleChaperone()
	{
		if (isLocalPlayer)
		{
			_chaperoneScript.ToggleChaperone();
		}

	}
	#endregion
}
