using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;


public class GameManager : NetworkBehaviour
{
    public static GameManager Instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public GameObject AvatarPrefab;
    public GameObject AvatarRed;
    public GameObject AvatarGreen;
    public Material MaterialRed;
    public GameObject Communicator;

    [SyncVar]
    public bool AvatarRedTaken;
    [SyncVar]
    public bool AvatarRedControlled;
    [SyncVar]
    public bool AvatarGreenTaken;
    [SyncVar]
    public bool AvatarGreenControlled;

    void Awake () {
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
        if (Input.GetKeyDown(KeyCode.N))
        {
            ServerManager.Instance.StopServer();
            Application.LoadLevel(Application.loadedLevel);
        }
    }


}
