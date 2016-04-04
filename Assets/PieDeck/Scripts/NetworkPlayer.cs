using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.VR;

public class NetworkPlayer : NetworkBehaviour
{
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
    private bool timeStamped;

	private ParticleSystem[] ps;

	void Start () {
		ps = GetComponentsInChildren<ParticleSystem>(true);
		if (!isServer)
		{
			_vrController = GameObject.Find("OVRPlayerController");
			_vrControllerScript = _vrController.GetComponent<OVRPlayerController>();
			_chaperoneScript = _vrController.GetComponent<Chaperone>();

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer)
		{
			//Update Position and Rotation
			
			transform.rotation = _vrController.transform.rotation;
			CmdUpdateOrientation(transform.rotation);

			ColorId = 1;
			ps[0].gameObject.SetActive(true);
			transform.FindChild("Body").gameObject.SetActive(false);
			transform.FindChild("Orientation").gameObject.SetActive(false);
		}
		if (isServer)
		{
			transform.name = "" + connectionToClient.connectionId;
			GetComponent<CharacterController>().enabled = false;
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

    void CalculateVRPos()
    {
        _vrController.transform.position = transform.position;
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
