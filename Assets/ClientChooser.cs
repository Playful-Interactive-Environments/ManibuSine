using UnityEngine;
using System.Collections;

public class ClientChooser : MonoBehaviour {
    private ClientChooser instance;
    public ClientChooser Instance { get { return instance; } }

    public enum ClientType
    {
        RenderClientFloor,
        RenderClientWall,
        VRClient
    }

    public ClientType clientType;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
