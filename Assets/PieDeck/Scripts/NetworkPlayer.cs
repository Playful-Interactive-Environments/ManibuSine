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

	private ParticleSystem[] ps;

	void Start () {
		ps = GetComponentsInChildren<ParticleSystem>(true);
		if (!isServer)
		{
			_vrControllerScript = GameObject.Find("OVRPlayerController").GetComponent<OVRPlayerController>();
			_vrController = GameObject.Find("OVRPlayerController");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer)
		{
			//Update Position and Rotation
			_vrController.transform.position = transform.position;
			transform.rotation = _vrController.transform.rotation;
			CmdUpdateOrientation(transform.rotation);

			ColorId = 1;
			ps[0].gameObject.SetActive(true);
			transform.FindChild("Body").gameObject.SetActive(false);
			transform.FindChild("Orientation").gameObject.SetActive(false);

			if (Input.GetKeyDown(KeyCode.R))
			{
				_vrControllerScript.ResetOrientation();

			}
		}
		if (isServer)
		{
			transform.name = "" + connectionToClient.connectionId;
			GetComponent<CharacterController>().enabled = false;
			GetComponent<CapsuleCollider>().enabled = false;
			if (connectionToClient.connectionId == 1)
			{
				ServerManager.Instance.PlayerOne = gameObject;
			}
			if (connectionToClient.connectionId == 2)
			{
				ServerManager.Instance.PlayerTwo = gameObject;

			}
			if (ControllingPlayer != null)
			{
				transform.position = ControllingPlayer.transform.position;
				position = transform.position;
			}
			

		}

	}

	public void ResetOrientation()
	{
		RpcRecalibrateDevice();
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
		_vrControllerScript.ResetOrientation();
	}
	#endregion
}
