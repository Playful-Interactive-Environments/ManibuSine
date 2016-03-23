using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayer : NetworkBehaviour
{
	public GameObject ControllingPlayer;
	[SyncVar] private Vector3 position;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer)
		{
			GameObject.Find("OVRPlayerController").transform.position = transform.position;
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
}
