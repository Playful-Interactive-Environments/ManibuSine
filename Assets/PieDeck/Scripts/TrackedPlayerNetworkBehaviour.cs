using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TrackedPlayerNetworkBehaviour : NetworkBehaviour {

	[SyncVar]
	public bool HasPlayerOne;
	[SyncVar]
	public bool HasPlayerTwo;

	public GameObject ControlledPlayer;

	void Start () {

	}
	

	void Update () {
		if (ServerManager.Instance.isServer)
		{
			NetworkServer.Spawn(gameObject);
			this.transform.FindChild("Server").gameObject.SetActive(true);
			this.transform.FindChild("Client").gameObject.SetActive(false);
		}
		if (ServerManager.Instance.isClient)
		{
			this.transform.FindChild("Server").gameObject.SetActive(false);
			if (HasPlayerOne || HasPlayerTwo)
			{
				this.transform.FindChild("Client").gameObject.SetActive(false);
			}
			if (!HasPlayerOne && !HasPlayerTwo)
			{
				this.transform.FindChild("Client").gameObject.SetActive(true);
			}
		}
	}
	void OnDestroy()
	{
		if (ServerManager.Instance.isServer)
		{
			if (HasPlayerOne)
			{
				ServerManager.Instance.ButtonPlayerOne.interactable = true;
			}
			if (HasPlayerTwo)
			{
				ServerManager.Instance.ButtonPlayerTwo.interactable = true;
			}
		}
		

	}
	void OnMouseDown()
	{
			ServerManager.Instance.CurrentTrackedPlayer = gameObject;
			StartCoroutine("ChangeColor");
	}

	IEnumerator ChangeColor()
	{
		Color current = this.transform.FindChild("Server").GetComponent<MeshRenderer>().material.color;
		this.transform.FindChild("Server").GetComponent<MeshRenderer>().material.color = Color.black;
		yield return new WaitForSeconds(3f);
		this.transform.FindChild("Server").GetComponent<MeshRenderer>().material.color = current;

	}


}
