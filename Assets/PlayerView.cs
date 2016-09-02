using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour {

    public NetworkPlayer player;
    public RenderTexture renderTexture;
    private Text text;
    private RawImage rawImage;
	// Use this for initialization
	void Start () {
        rawImage = GetComponent<RawImage>();
        rawImage.enabled = false;
        text = GetComponentInChildren<Text>();
        text.enabled = false;
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
        text.enabled = true;
    }

    public void DisconnectPlayer()
    {
        Camera cam = this.player.GetComponentInChildren<Camera>();
        cam.enabled = false;
        rawImage.enabled = false;
        text.enabled = false;
        this.player = null;

    }
}
