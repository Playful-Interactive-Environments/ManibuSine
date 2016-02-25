using UnityEngine;
using System.Collections;

public class DrawFootprint : MonoBehaviour
{


	public GameObject footPrintContainer;
    private GameObject _container;
	public GameObject footPrint;
	public GameObject[] foots;

	private Vector3 lastPos;
	private float minDistance = 2;
	private float minSqrDistance;
	private int footCount = 0;

	void Start () {
		lastPos = transform.position;
		minSqrDistance = Mathf.Pow (minDistance, 2);
        _container = Instantiate(footPrintContainer, this.transform.position, Quaternion.identity) as GameObject;
	}
	
	void FixedUpdate () {

		lastPos.y = transform.position.y;

		Vector3 direction = lastPos - transform.position;

		if (direction.sqrMagnitude >= minSqrDistance) {

			GameObject obj = Instantiate (footPrint);
			Vector3 pos = foots[footCount % 2].transform.position;
			obj.transform.position = pos;
			obj.GetComponent<Renderer> ().material = GetComponent<Renderer> ().material;
			obj.SetActive (true);
			obj.transform.parent = _container.transform;
			lastPos = transform.position;
			footCount++;
		}

		deleteOldFootprints ();
	}

	void deleteOldFootprints () {
		Transform[] obj = _container.transform.gameObject.GetComponentsInChildren<Transform> ();
		int maxItemCount = 20;
		for (int i = 1; i < obj.Length; i++) {
			Vector3 scale = obj [i].localScale;
			scale *= 0.999F;
			obj [i].localScale = scale;
		}

		if (obj.Length > maxItemCount) {
			for (int i = 1; i < obj.Length - maxItemCount; i++) {
				Destroy (obj[i].gameObject);
			}
		}

	}
}
