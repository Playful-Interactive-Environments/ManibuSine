using UnityEngine;
using System.Collections;

public class CreateEmotion : MonoBehaviour {


	public GameObject footprints;
	public GameObject happy;
	public GameObject sad;
	public GameObject anger;
	public GameObject scared;

	public GameObject emotionTrackerPrefab;
	public Material[] materials;

	void Update () {
		if ((int)Random.Range (0, 1000) == 1) {

			Transform[] child = footprints.GetComponentsInChildren<Transform> ();

			if (child.Length > 20) {

				GameObject c = child [(int)Random.Range (1, child.Length - 15)].gameObject;

				Vector3 pos = c.transform.position;
				Material m = materials [(int)Random.Range (0, materials.Length - 0.00001F)];

				Destroy (c);

				showEmotion (m, pos);

				GameObject tracker = Instantiate (emotionTrackerPrefab);
				tracker.transform.position = pos; 
				tracker.GetComponent<Renderer> ().material = m;
				tracker.transform.parent = emotionTrackerPrefab.transform.parent;
				tracker.SetActive (true);
			}
		}
	}

	void showEmotion (Material m, Vector3 pos) {
		if (m.name.Contains ("Happy")) {
			if (!happy.activeSelf) 
			{
				happy.transform.position = pos;
				happy.SetActive (true);
			}
		} else if (m.name.Contains ("Anger")) {
			if (!anger.activeSelf) 
			{
				anger.transform.position = pos;
				anger.SetActive (true);
			}
		} else {
		}
	}
}
