using UnityEngine;
using System.Collections;

public class DeactiveByTime : MonoBehaviour {

	float setActiveTime;


	void Start () {
		setActiveTime = -1;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.activeSelf && setActiveTime == -1)
			setActiveTime = Time.time;

		if (gameObject.activeSelf && Time.time - setActiveTime > 10) {
			gameObject.SetActive (false);
			setActiveTime = -1;
		}
	}
}
