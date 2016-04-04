using UnityEngine;
using System.Collections;
using System.Xml;

public class DisplayScript : MonoBehaviour
{

	void Awake () {
		if (Display.displays.Length > 1)
		{
			Display.displays[1].Activate();
		}
		


	}
	
	// Update is called once per frame
	void Start () {
	   
	}
}
