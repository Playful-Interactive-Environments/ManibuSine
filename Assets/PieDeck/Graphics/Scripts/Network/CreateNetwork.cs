using UnityEngine;
using System.Collections;

public enum EmotionState  {
	Happy,
	Sad,
	Anger,
	Scared
}

public class CreateNetwork : MonoBehaviour {

	public GameObject startCube;
	public GameObject player;
	public GameObject subNetwork;

	private Vector3 lastPos;
	private float minSqrDistance;
	private float minSqrScaleDistance;
	private float minDistance;
	private GameObject lastCube;
	private float drawTime;
	private EmotionState emotion = EmotionState.Happy;

	void Start () {
		lastPos = player.transform.position;
		minDistance = startCube.transform.localScale.x * 0.95F;
		minSqrDistance = Mathf.Pow (minDistance, 2);
		minSqrScaleDistance = Mathf.Pow (0.1F, 2);
		lastCube = createInstance ();
		drawTime = Time.time;
	}

	private GameObject createInstance() {
		lastCube = Instantiate (startCube);
		lastCube.SetActive (true);
		return lastCube;
	}
	
	void FixedUpdate ()
	{
		Vector3 pos = new Vector3 ();
		bool drawCube = false;

		if (lastPos.y != player.transform.position.y)
			lastPos.y = player.transform.position.y;

		Vector3 direction = lastPos - player.transform.position;
		float deltaDrawTime = Time.time - drawTime;

		if (direction.sqrMagnitude >= minSqrScaleDistance) {
			Vector3 size = lastCube.transform.localScale;
			size.x = (Mathf.Abs (direction.normalized.x) + 1) * (deltaDrawTime + 0.1F);
			size.y = (Mathf.Abs (direction.normalized.y) + 1) * (deltaDrawTime + 0.1F);
			size.z = (Mathf.Abs (direction.normalized.z) + 1) * (deltaDrawTime + 0.1F);
			lastCube.transform.localScale = size;
		}

		if (direction.sqrMagnitude >= minSqrDistance) {
			drawCube = true;

			pos.x = player.transform.position.x;
			pos.y = lastCube.transform.position.y;
			pos.z = player.transform.position.z;


			lastPos = player.transform.position;
		} else if (deltaDrawTime > 2) {
			drawCube = true;

			pos.x = lastCube.transform.position.x;
			pos.y = lastCube.transform.position.y;
			pos.z = lastCube.transform.position.z;
		}

		if (drawCube) {
			lastCube.transform.parent = subNetwork.transform;

			lastCube = createInstance ();

			lastCube.transform.position = pos;
			drawTime = Time.time;
		}

		Vector3 netSize = subNetwork.transform.localScale;
		netSize *= 1.001F;
		subNetwork.transform.localScale = netSize;


		Transform[] obj = subNetwork.GetComponentsInChildren<Transform> ();
		int maxItemCount = 200;
		if (obj.Length > maxItemCount) {
			for (int i = 1; i < obj.Length - maxItemCount; i++) {
				Destroy (obj[i].gameObject);
			}
		}
	}
}
