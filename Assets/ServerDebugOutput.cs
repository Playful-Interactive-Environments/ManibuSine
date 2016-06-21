using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerDebugOutput : MonoBehaviour {
    private static ServerDebugOutput instance;
    public static ServerDebugOutput Instance
    {
        get { return instance; }
    }

    Text uiText;

	// Use this for initialization
	void Awake () {
        instance = this;
	}
    void Start() {
        uiText = GetComponentInChildren<Text>();
    }
	
    public void SetText(string text) {
        if (uiText == null)
            return;

        uiText.text = text;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
