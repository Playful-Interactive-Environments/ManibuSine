using UnityEngine;
using System.Collections;

public class ScreenManager : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate each.
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
            Display.displays[1].SetParams(Display.displays[1].systemWidth, Display.displays[1].systemHeight, Display.displays[0].systemWidth, 0);
        }
            
        if (Display.displays.Length > 2)
            Display.displays[2].Activate();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
