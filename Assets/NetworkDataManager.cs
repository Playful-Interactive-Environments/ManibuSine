using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkDataManager : NetworkBehaviour {
    private static NetworkDataManager instance;
    public static NetworkDataManager Instance { get { return instance;}}

    void Awake()
    {
        instance = this;
    }

    public delegate void SimpleEvent();

    //SYNCEVENTS
    [SyncEvent]
    public event SimpleEvent EventShoot;

    [Command]
    public void CmdShoot()
    {
        if(EventShoot != null)
        {
            EventShoot();
        }
    }
    



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (ServerManager.Instance.isServer)
        {
            NetworkServer.Spawn(gameObject);
        }
	}
}
