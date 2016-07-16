﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ServerManager : NetworkManager
{

	public string ConnectionIP;
	public int ConnectionPort = 7777;
	public bool ClientConnected = false;
    //public TextMesh debugTextClient;
	public static ServerManager Instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
	public bool isServer;
	public bool isClient;
	public Text debugTextServer;
    public GameObject SoundManager;
    public GameObject CanonStationLeft;
    public GameObject CanonStationRight;
    public GameObject TargetTransform;
    public GameObject RotationTransform;
    public GameObject SteeringStation;
    public GameObject PublicPlayer;

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

	void Update()
	{
	   if(Input.GetKeyDown(KeyCode.Escape))
        {
            StopServer();
            NetworkServer.Reset();
            Application.Quit();
        }
	}

    public void RestartApplication()
    {
        StopServer();
        NetworkServer.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void SpawnEntityAtPrefabPosition(GameObject prefab) {
        GameObject obj = Instantiate(prefab) as GameObject;
        NetworkServer.Spawn(obj);
    }

    public void SpawnEntity(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, new Vector3(-6000, Random.Range(200, 2000), Random.Range(-1500, 1500)), Quaternion.identity) as GameObject;
        NetworkServer.Spawn(obj);
    }

    public void SpawnEntityAt(GameObject prefab, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (!isServer)
            return;

        GameObject obj = Instantiate(prefab, spawnPosition, spawnRotation) as GameObject;
        NetworkServer.Spawn(obj);
    }


    public void SpawnPublicPlayer(MaSineTrackedPlayer tp) {
        if (!isServer)
            return;

        GameObject obj = Instantiate(PublicPlayer, tp.transform.position, Quaternion.identity) as GameObject;



        tp.PublicPlayer = obj.GetComponent<PublicPlayer>();

        obj.GetComponent<PublicPlayer>().ControllingPlayer = tp;
        NetworkServer.Spawn(obj);
    }


    public void StartupHost()
	{
		Admin.Instance.ButtonPlayerOne.gameObject.SetActive(false);
		Admin.Instance.ButtonPlayerTwo.gameObject.SetActive(false);
        //Instantiate(SoundManager, new Vector3(0,0,0), Quaternion.identity);


        // TODO: check why this doesn't work
        //Instantiate(TargetTransform);
        //Instantiate(SteeringStation);
        //Instantiate(CanonStation);
        SetPort();
		StartServer();
		isServer = true;
		NetworkServer.SpawnObjects();

        SpawnEntityAtPrefabPosition(SteeringStation);
        SpawnEntityAtPrefabPosition(TargetTransform);
        SpawnEntityAtPrefabPosition(RotationTransform);
        SpawnEntityAtPrefabPosition(CanonStationLeft);
        SpawnEntityAtPrefabPosition(CanonStationRight);
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

        ShipManager.Instance.Initialize();
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
		//debugTextClient.text = "ClientConnected";
	}

	public override void OnStartClient(NetworkClient client)
	{
		base.OnStartClient(client);
		//debugTextClient.text = "Client Started";
	}

	public override void OnStopClient()
	{
		base.OnStopClient();
		//debugTextClient.text = "Server Stopped";
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
