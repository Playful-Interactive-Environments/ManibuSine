using UnityEngine;
using System.Collections;

public class ClientManager : MonoBehaviour {

	
	void Start () {
	    GetComponent<ServerManager>().JoinGame();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
