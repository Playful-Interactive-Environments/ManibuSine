using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour {

    public NetworkPlayer player;
    public RenderTexture renderTexture;
    private RawImage rawImage;
	// Use this for initialization
	void Start () {
        rawImage = GetComponent<RawImage>();
        rawImage.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ConnectPlayer(NetworkPlayer player)
    {
        this.player = player;
        Camera cam = player.GetComponentInChildren<Camera>();
        cam.targetTexture = (RenderTexture)rawImage.texture;
        cam.enabled = true;
        rawImage.enabled = true;

    }

    public void DisconnectPlayer()
    {
        Camera cam = this.player.GetComponent<Camera>();
        cam.enabled = true;
        rawImage.enabled = true;
        this.player = null;

    }
}
