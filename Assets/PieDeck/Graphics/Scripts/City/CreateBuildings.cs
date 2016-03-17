using UnityEngine;
using System.Collections;

public class CreateBuildings : MonoBehaviour {

	public GameObject MainBuilding;
	public int BuildingCount = 10;
    public EnvironmentSetup environmentSetup;

	void Start () {
		int rowCount = (int)Mathf.Sqrt (BuildingCount);
		for (int i = 0; i < rowCount; i++) {
			for (int j = 0; j < rowCount; j++) {
				Vector3 scale = new Vector3 ();
				scale.x = Random.Range (environmentSetup.MainBuildingWidth / 3.0F, environmentSetup.MainBuildingWidth);
				scale.y = Random.Range (600, 3500);
				scale.z = Random.Range (environmentSetup.MainBuildingDepth /3.0F, environmentSetup.MainBuildingDepth);

				Vector3 pos = new Vector3 ();
				pos.y = -environmentSetup.MainBuildingHeight + scale.y/2; 
				pos.x = (i - rowCount/2) * (environmentSetup.BuildingDistanceX) + environmentSetup.MainBuildingCenterX;
				pos.z = (j - rowCount/2) * (environmentSetup.BuildingDistanceY) + environmentSetup.MainBuildingCenterY;

				if (pos.x == environmentSetup.MainBuildingCenterX && pos.z == environmentSetup.MainBuildingCenterY)
					continue;

				GameObject b = Instantiate (MainBuilding);
				b.transform.position = pos;
				b.transform.localScale = scale;
				b.transform.parent = MainBuilding.transform.parent;

			}
		}
	}
}
