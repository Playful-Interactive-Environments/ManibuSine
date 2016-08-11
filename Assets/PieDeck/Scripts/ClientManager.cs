using UnityEngine;
using System.Collections;

public class ClientManager : MonoBehaviour {

	
	void Start () {
        InvokeRepeating("TryConnecting", 2, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		//if (!ServerManager.Instance.isNetworkActive)
		//{
		//	GetComponent<ServerManager>().JoinGame();
		//}
	}

    void TryConnecting()
    {
        if (ServerManager.Instance.isNetworkActive)
        {
            CancelInvoke("TryConnecting");
            return;
        }
            
        GetComponent<ServerManager>().JoinGame();
    }
}
