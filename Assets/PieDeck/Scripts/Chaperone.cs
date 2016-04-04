using UnityEngine;
using System.Collections;

public class Chaperone : MonoBehaviour {

	private GameObject CameraRigVR;
	private GameObject CameraRigAR;
	private GameObject UserHead;
	private bool _chaperoneActive;
	void Start () {
		CameraRigVR = GameObject.Find("OVRCameraRig");
		CameraRigAR = GameObject.Find("OVRCameraRigAR");
		UserHead = GameObject.Find("UserHead");

		CameraRigAR.gameObject.SetActive(false);
		UserHead.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Cancel"))
		{
			ToggleChaperone();
		}
	}

	public void ToggleChaperone()
	{
		if (_chaperoneActive)
		{
			CameraRigVR.gameObject.SetActive(true);
			CameraRigAR.gameObject.SetActive(false);
			UserHead.gameObject.SetActive(false);
			_chaperoneActive = false;
		}
		else
		{
			CameraRigVR.gameObject.SetActive(false);
			CameraRigAR.gameObject.SetActive(true);
			UserHead.gameObject.SetActive(true);
			_chaperoneActive = true;
		}
	}
}
