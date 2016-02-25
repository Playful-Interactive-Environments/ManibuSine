using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Communicator : NetworkBehaviour {

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
        if (myTransform.name == "" || myTransform.name == "Player(Clone)")
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
        string uniqueName = "Player " + playerNetID;
        return uniqueName;
    }

    [Command]
    void CmdTellServerMyIdentity(string name)
    {
        playerUniqueIdentity = name;
    }

    [Command]
    public void CmdTellServer()
    {
        Debug.Log("Green Taken");
    }
}
