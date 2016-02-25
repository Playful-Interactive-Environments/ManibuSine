using UnityEngine;
using System.Collections;

public class CreateBuildings : MonoBehaviour {

	public GameObject MainBuilding;
	public int BuildingCount = 10;
    public int ScaleFactor;

	void Start () {
		int rowCount = (int)Mathf.Sqrt (BuildingCount);
		for (int i = 0; i < rowCount; i++) {
			for (int j = 0; j < rowCount; j++) {
				Vector3 scale = new Vector3 ();
				scale.x = Random.Range (4, 18) * ScaleFactor;
				scale.y = Random.Range (400, 2500);
				scale.z = Random.Range (4, 12) * ScaleFactor;

				Vector3 pos = new Vector3 ();
				pos.y = -500; 
				pos.x = (i - rowCount/2) * 1500 + 960;
				pos.z = (j - rowCount/2) * 800 + 540;

				if (pos.x == 960 && pos.z == 540)
					continue;

				GameObject b = Instantiate (MainBuilding);
				b.transform.position = pos;
				b.transform.localScale = scale;
				b.transform.parent = MainBuilding.transform.parent;

			}
		}
	}
}
