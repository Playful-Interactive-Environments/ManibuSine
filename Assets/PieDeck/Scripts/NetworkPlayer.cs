using UnityEngine;
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
    
    public GameObject head;

	public GameObject ControllingPlayer;
	[SyncVar]
	public Quaternion rotation;
	[SyncVar]
	public Vector3 position;
	public int ColorId = 2;
	private GameObject _vrController;
	private OVRPlayerController _vrControllerScript;
	private Chaperone _chaperoneScript;

	private Vector3 _previousPos;
	private Vector3 _currentPos;
	private float distance;
	private float _timePassed;
	private bool _staticPos;
    
    [SyncVar]
    private bool laserTrackingActivated;
    private float shipSpeed = 3;

    void Start () {
		if (!isServer)
		{
			_vrController = GameObject.Find("OVRPlayerController");
			_vrControllerScript = _vrController.GetComponent<OVRPlayerController>();
			_chaperoneScript = _vrController.GetComponent<Chaperone>();
        }
        else
        {
            if (ControllingPlayer != null)
            {
                laserTrackingActivated = true;
            }
            else
            {
                laserTrackingActivated = false;
            }
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

    private void ShipEnteredEvent(IEventTrigger waypoint)
    {
        if (!isLocalPlayer && waypoint != null)
            return;

        CmdSetLevelState(waypoint.GetID());
    }


	
	// Update is called once per frame
    void Update()
    {
        //if (isServer)
        //{
        //    if (ControllingPlayer != null)
        //    {
        //        laserTrackingActivated = true;
        //    }
        //    else
        //    {
        //        laserTrackingActivated = false;
        //    }
        //}
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

            ColorId = 1;
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
                position = transform.position;
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
        //RpcSpawnBullet();
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
    //----------------------------------------------------------------
    //----------------------------------------------------------------


    void LocalPlayerMovement()
    {
        transform.position = _vrController.transform.position;
    }

	void CalculateVRPos()
	{
        float minMoveDistance = 0.03f;
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
			_vrController.transform.position = Vector3.Slerp(_previousPos, transform.position, 0.01f);
			_staticPos = false;
		}
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

	#region Sound Triggers
	void OnTriggerEnter(Collider other)
	{
		if (other.transform.name == "ambienceTrigger")
		{
			SoundManager.Instance.PlaySound("ambience", "in", 1f);
		}
		if (other.transform.name == "tribalTrigger")
		{
			SoundManager.Instance.PlaySound("tribal", "in", 1f);
		}
		if (other.transform.name == "trumpetTrigger")
		{
			SoundManager.Instance.PlaySound("trumpet", "in", 1f);
		}
        if (other.transform.name == "guitarTrigger")
        {
            SoundManager.Instance.PlaySound("guitar", "in", 1f);
        }
    }
	void OnTriggerExit(Collider other)
	{
		if (other.transform.name == "ambienceTrigger")
		{
			SoundManager.Instance.PlaySound("ambience", "out", 0f);
		}
		if (other.transform.name == "tribalTrigger")
		{
			SoundManager.Instance.PlaySound("tribal", "out", 0f);
		}
		if (other.transform.name == "trumpetTrigger")
		{
			SoundManager.Instance.PlaySound("trumpet", "out", 0f);
		}
        if (other.transform.name == "guitarTrigger")
        {
            SoundManager.Instance.PlaySound("guitar", "out", 0f);
        }
    }
	#endregion
}
