using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine.Networking;
using UnityEngine.VR;
using VRStandardAssets.Utils;


public class AvatarPlayer : NetworkBehaviour
{

    [SyncVar]private Vector3 _syncPos;
	[SyncVar] public string playerUniqueName;
	private NetworkInstanceId playerNetID;
	private Transform myTransform;
	public bool playerActive;
    public GameObject controllingPlayer;
    public GameObject ServerBody;
    public GameObject ClientBody;
    public int ColorId = 2;


	void Awake()
	{
        ClientBody.gameObject.SetActive(false);
        ServerBody.gameObject.SetActive(true);
        myTransform = transform;
		playerActive = false;
	}

	void Update()
	{
		this.transform.position = _syncPos;

		if (isServer && playerActive && controllingPlayer != null)
		{
			_syncPos = controllingPlayer.transform.position;
		}
		if (isLocalPlayer)
		{
			if (Input.GetButtonDown("Cancel"))
			{

			}

		}
		if (myTransform.name == "" || myTransform.name == "Avatar(Clone)")
		{
			myTransform.name = playerUniqueName;

		}
		if (myTransform.name == "Avatar Red")
		{
			GameManager.Instance.AvatarRed = this.gameObject;
		    GetComponentInChildren<MeshRenderer>().material = GameManager.Instance.MaterialRed;
            ColorId = 0;

        }
		if (myTransform.name == "Avatar Green")
		{
			GameManager.Instance.AvatarGreen = this.gameObject;
            ColorId = 1;

        }
	    if (isClient)
	    {
            ClientBody.gameObject.SetActive(true);
            ServerBody.gameObject.SetActive(true);
        }
	    if (isServer)
	    {
            ClientBody.gameObject.SetActive(false);
            ServerBody.gameObject.SetActive(true);
        }

    }

    public void setActive(bool active, GameObject player)
	{
		playerActive = active;
		controllingPlayer = player;
	}

	public bool getActive()
	{
		return playerActive;
	}

	public override void OnStartLocalPlayer()
	{
		myTransform.name = playerUniqueName;
	}

}



