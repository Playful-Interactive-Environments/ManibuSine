using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SoundManager : NetworkBehaviour {

	public static SoundManager Instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
	[SyncVar]
	Vector3 ambienceVars;
	[SyncVar]
	Vector3 tribalVars;
	[SyncVar]
	Vector3 trumpetVars;
    [SyncVar]
    private Vector3 guitarVars;

	public GameObject Ambience, Tribal, Trumpet, Guitar;
	private AudioSource _ambience, _tribal, _trumpet, _guitar;
	void Awake()
	{
		_ambience = Ambience.GetComponent<AudioSource>();
		_tribal = Tribal.GetComponent<AudioSource>();
		_trumpet = Trumpet.GetComponent<AudioSource>();
        _guitar = Guitar.GetComponent<AudioSource>();
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
		void Start ()
		{

			if (ServerManager.Instance.isClient)
			{
				_ambience.time = ambienceVars.x;
				_ambience.volume = ambienceVars.y;
				_tribal.time = tribalVars.x;
				_tribal.volume = tribalVars.y;
				_trumpet.time = trumpetVars.x;
				_trumpet.volume = trumpetVars.y;
                _guitar.time = guitarVars.x;
                _guitar.volume = guitarVars.y;
            }
			_ambience.Play();
			_tribal.Play();
			_trumpet.Play();
            _guitar.Play();
		}
		
		void Update ()
		{
			if (ServerManager.Instance.isServer)
			{
				NetworkServer.Spawn(gameObject);
				ambienceVars.x = _ambience.time;
				ambienceVars.y = _ambience.volume;
				tribalVars.x = _tribal.time;
				tribalVars.y = _tribal.volume;
				trumpetVars.x = _trumpet.time;
				trumpetVars.y = _trumpet.volume;
                guitarVars.x = _guitar.time;
                guitarVars.y = _guitar.volume;
        }
	}

	public void PlaySound(string name, string state, float volume)
	{
		if (name == "ambience" && state == "in")
		{
			iTween.AudioTo(gameObject, iTween.Hash("audiosource", _ambience, "volume", volume));
		}
		if (name == "ambience" && state == "out")
		{
			iTween.AudioTo(gameObject, iTween.Hash("audiosource", _ambience, "volume", volume));
		}
		if (name == "tribal" && state == "in")
		{
			iTween.AudioTo(gameObject, iTween.Hash("audiosource", _tribal, "volume", volume));
		}
		if (name == "tribal" && state == "out")
		{
			iTween.AudioTo(gameObject, iTween.Hash("audiosource", _tribal, "volume", volume));
		}
		if (name == "trumpet" && state == "in")
		{
			iTween.AudioTo(gameObject, iTween.Hash("audiosource", _trumpet, "volume", volume));
		}
		if (name == "trumpet" && state == "out")
		{
			iTween.AudioTo(gameObject, iTween.Hash("audiosource", _trumpet, "volume", volume));
		}
        if (name == "guitar" && state == "in")
        {
            iTween.AudioTo(gameObject, iTween.Hash("audiosource", _guitar, "volume", volume));
        }
        if (name == "guitar" && state == "out")
        {
            iTween.AudioTo(gameObject, iTween.Hash("audiosource", _guitar, "volume", volume));
        }

        if (isServer)
		{
			SendToClient(name, state, volume);
		}
		
	}
	public void SendToClient(string name, string state, float volume)
	{
		RpcPlaySound(name, state, volume);
	}

	[ClientRpc]
	void RpcPlaySound(string name, string state, float volume)
	{
		PlaySound(name, state, volume);
	}
	
}
