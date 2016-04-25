using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

public class ServerManager : NetworkManager
{

	public string ConnectionIP;
	public int ConnectionPort = 7777;
	public bool ClientConnected = false;
	public TextMesh debugTextClient;
	public static ServerManager Instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
	public bool isServer;
	public bool isClient;
	public Text debugTextServer;
    public GameObject SoundManager;
    public GameObject NetworkDataManager;



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
	void Update()
	{
	   
	}
	public void StartupHost()
	{
		Admin.Instance.ButtonPlayerOne.gameObject.SetActive(false);
		Admin.Instance.ButtonPlayerTwo.gameObject.SetActive(false);
	    Instantiate(SoundManager, new Vector3(0,0,0), Quaternion.identity);
        Instantiate(NetworkDataManager, new Vector3(0, 0, 0), Quaternion.identity);
        SetPort();
		StartServer();
		isServer = true;
		NetworkServer.SpawnObjects();
	}

	public void StopHosting()
	{
		StopServer();
		NetworkServer.Reset();
	}

	void SetIPAddress()
	{
		networkAddress = ConnectionIP;
	}
	void SetPort()
	{
		networkPort = ConnectionPort;
	}
	#region Server 
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
	public override void OnServerConnect(NetworkConnection conn)
	{
		if (conn.connectionId == 1)
		{
			Admin.Instance.ButtonPlayerOne.gameObject.SetActive(true);
			Admin.Instance.ButtonPlayerOne.interactable = true;
		}
		if (conn.connectionId == 2)
		{

			Admin.Instance.ButtonPlayerTwo.gameObject.SetActive(true);
			Admin.Instance.ButtonPlayerTwo.interactable = true;
		}
		debugTextServer.text = "Client " + conn.connectionId + " connected.";
		
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		base.OnServerDisconnect(conn);
		if (conn.connectionId == 1)
		{
			Admin.Instance.ButtonPlayerOne.gameObject.SetActive(false);
			Admin.Instance.ButtonPlayerOne.interactable = true;
		}
		if (conn.connectionId == 2)
		{
			Admin.Instance.ButtonPlayerTwo.gameObject.SetActive(false);
			Admin.Instance.ButtonPlayerTwo.interactable = true;
		}
		debugTextServer.text = "Client " + conn.connectionId + " disconnected.";
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		base.OnServerAddPlayer(conn, playerControllerId);
		
	}

	public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
	{
		
		base.OnServerRemovePlayer(conn, player);
	}

	#endregion

	#region Client
	public void JoinGame()
	{
		SetIPAddress();
		SetPort();
		StartClient();
		isClient = true;

	}

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
