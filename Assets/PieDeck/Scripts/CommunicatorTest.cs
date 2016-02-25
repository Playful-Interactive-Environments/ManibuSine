using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CommunicatorTest : NetworkBehaviour {

	[SyncVar]
	private string playerUniqueIdentity;
	private NetworkInstanceId playerNetID;
	private Transform myTransform;
	public override void OnStartLocalPlayer()
	{
		GetNetIdentity();
		SetIdentity();
	}

	void Awake () {
		myTransform = transform;
	}

	void Start()
	{
		GameManager.Instance.Communicator = this.gameObject;
	}
	
	void Update () {
		if (myTransform.name == "" || myTransform.name == "CommunicatorTest (Clone)")
		{
			SetIdentity();
		}
	}
	[Client]
	void GetNetIdentity()
	{
		playerNetID = GetComponent<NetworkIdentity>().netId;
		CmdTellServerMyIdentity(MakeUniqueIdentity());
	}

	void SetIdentity()
	{
		if (!isLocalPlayer)
		{
			myTransform.name = playerUniqueIdentity;
		}
		else
		{
			myTransform.name = MakeUniqueIdentity();
		}
	}

	string MakeUniqueIdentity()
	{
		string uniqueName = "CommunicatorTest " + playerNetID;
		return uniqueName;
	}

	[Command]
	void CmdTellServerMyIdentity(string name)
	{
		playerUniqueIdentity = name;
	}

	[Command]
	public void CmdGreenControlled()
	{
		GameManager.Instance.AvatarGreenControlled = true;
	}
	[Command]
	public void CmdRedControlled()
	{
		GameManager.Instance.AvatarRedControlled = true;
	}
}
