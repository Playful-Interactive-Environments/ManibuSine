using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.VR;
using UnityPharus;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    private float headTilt;
    
    public GameObject head;
    public UI_HeasUpCompas compasPrefab;

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

	private ParticleSystem[] ps;
    [SyncVar]
    private bool laserTrackingActivated;
    private float shipSpeed = 3;

    void Start () {
		ps = GetComponentsInChildren<ParticleSystem>(true);

		if (!isServer)
		{
			_vrController = GameObject.Find("OVRPlayerController");
			_vrControllerScript = _vrController.GetComponent<OVRPlayerController>();
			_chaperoneScript = _vrController.GetComponent<Chaperone>();


            UI_HeasUpCompas compas = Instantiate(compasPrefab);
            compas.GetComponent<UI_HeasUpCompas>().SetPlayer(this.transform);

		}
	}
	
	// Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            if(ControllingPlayer != null)
            {
                laserTrackingActivated = true;
            }
            else
            {
                laserTrackingActivated = false;
            }
        }
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
            ps[0].gameObject.SetActive(true);
            transform.FindChild("Body").gameObject.SetActive(false);
            //transform.FindChild("Orientation").gameObject.SetActive(false);

            //UNIVERSE MOVING INPUT
            if (Input.GetKey(KeyCode.I))
            {
                CmdMoveShipForward(500 * shipSpeed);
            }
            if (Input.GetKey(KeyCode.K))
            {
                CmdMoveShipBackward(-500 * shipSpeed);
            }
            if (Input.GetKey(KeyCode.J))
            {
                CmdRotateShipCCW(7 * shipSpeed);
            }
            if (Input.GetKey(KeyCode.L))
            {
                CmdRotateShipCW(-7 * shipSpeed);
            }
        }

        head.transform.rotation = Quaternion.Euler(headTilt, head.transform.rotation.eulerAngles.y, head.transform.rotation.eulerAngles.z);

        if (isServer)
        {
            transform.name = "" + connectionToClient.connectionId;
            //GetComponent<CharacterController>().enabled = false;
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
    // STUFF WE DID - we are forced to :(
    //----------------------------------------------------------------
    [Command]
    public void CmdShoot()
    {
        FindObjectOfType<Canon>().Shoot();
        //RpcSpawnBullet();
    }

    [ClientRpc]
    public void RpcSpawnBullet()
    {
        FindObjectOfType<Canon>().Shoot();
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
		_timePassed += Time.deltaTime;
		if (_timePassed > 3f)
		{
			_previousPos = transform.position;
			_timePassed = 0f;
		}
		_currentPos = transform.position;
		distance = Vector3.Distance(_previousPos, _currentPos);
		if (distance < 25f)
		{
			_vrController.transform.position = _previousPos;
			_staticPos = true;

		}
		if (distance >= 25f && !_staticPos)
		{
			_vrController.transform.position = transform.position;
		}
		if (distance >= 25f && _staticPos)
		{
			_vrController.transform.position = Vector3.Slerp(_previousPos, transform.position, 1f);
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
