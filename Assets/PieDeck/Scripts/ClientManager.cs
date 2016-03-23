using UnityEngine;
using System.Collections;

public class ClientManager : MonoBehaviour {

	
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!ServerManager.Instance.isNetworkActive)
		{
			GetComponent<ServerManager>().JoinGame();
		}
	}
}
