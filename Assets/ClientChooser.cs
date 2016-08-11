using UnityEngine;
using System.Collections;

public class ClientChooser : MonoBehaviour {
    private static ClientChooser instance;
    public static ClientChooser Instance { get { return instance; } }

    public enum ClientType {
        NotSet,
        RenderClientFloor,
        RenderClientWall,
        VRClient
    }

    public ClientType clientType;

    void Awake() {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        int hightest = QualitySettings.names.Length - 1;
        switch (clientType) {
            case ClientType.VRClient:
                break;
            case ClientType.RenderClientFloor:
                // set quality to max
                QualitySettings.SetQualityLevel(hightest);
                break;
            case ClientType.RenderClientWall:
                // set quality to max
                QualitySettings.SetQualityLevel(hightest);
                break;
            default:
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
