using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerManager : NetworkManager
{

	public string ConnectionIP;
	public int ConnectionPort = 7777;
	public bool ClientConnected = false;
    public TextMesh debugTextClient;
    public Text debugTextServer;
	public GameObject gameManagerPrefab;
    public GameObject AvatarPrefab;
    public GameObject AvatarRed;
    public GameObject AvatarGreen;
    public Material MaterialRed;
    public static ServerManager Instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    void Update()
    {


    }
    void Awake()
    {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    void Start()
	{
		
	}
	public void StartupHost()
	{
		SetPort();
		StartServer();
        GameObject Manager = Instantiate(gameManagerPrefab);
        AvatarRed = Instantiate(AvatarPrefab, transform.position, Quaternion.identity) as GameObject;
        AvatarGreen = Instantiate(AvatarPrefab, transform.position, Quaternion.identity) as GameObject;
        AvatarRed.GetComponent<AvatarPlayer>().playerUniqueName = "Avatar Red";
        AvatarGreen.GetComponent<AvatarPlayer>().playerUniqueName = "Avatar Green";
        AvatarRed.GetComponentInChildren<MeshRenderer>().material = MaterialRed;
        NetworkServer.Spawn(Manager);
        NetworkServer.Spawn(AvatarRed);
        NetworkServer.Spawn(AvatarGreen);
    }

	public void StopHosting()
	{
		StopServer();
	}

	public void JoinGame()
	{
		SetIPAddress();
		SetPort();
		StartClient();
	}

	void SetIPAddress()
	{
		networkAddress = ConnectionIP;
	}
	void SetPort()
	{
		networkPort = ConnectionPort;
	}
	#region Server Side
	public override void OnStartServer()
	{
		base.OnStartServer();
        
        debugTextServer.text = "Server Started";

	}

	public override void OnStopServer()
	{
		base.OnStopServer();
        debugTextServer.text = "Server Stopped";

	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		base.OnServerDisconnect(conn);
        debugTextServer.text = "Client Disconnected";
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		base.OnServerAddPlayer(conn, playerControllerId);
        debugTextServer.text = "Player joined " + playerControllerId;
	}

	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
	{
		base.OnServerRemovePlayer(conn, player);

	}

	#endregion

	#region Client Side
	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);
		debugTextClient.text = "ClientConnected";
	    

	}

	public override void OnStartClient(NetworkClient client)
	{
		base.OnStartClient(client);
		debugTextClient.text = "Client Started";

	}

	public override void OnStopClient()
	{
		base.OnStopClient();
		debugTextClient.text = "Server Stopped";
	}

    public void ReconnectClient()
    {
        StopClient();
        StartCoroutine("Reconnect");
    }

    IEnumerator Reconnect()
    {
        yield return new WaitForSeconds(1f);
        StartClient();

    }
	#endregion
}
