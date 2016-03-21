using UnityEngine;
using System.Collections;

public class CreateCars : MonoBehaviour {

	public ParticleSystem CarParticleSystem;
	//public int CarCount = 100;
    private EnvironmentSetup environmentSetup;

    public int rowCount;

    private float maxDistanceX;
    private float startPosX1;
    private float startPosX2;
    private float maxDistanceY;
    private float startPosY1;
    private float startPosY2;

    private float[] posX; 
	private float[] posZ;
    private float[] angle;
    private float[] angleRad;
    private int[] dir = { 1, 1, 3, 3, 0, 0, 2, 2 }; 

	void Start() {
        environmentSetup = GameObject.FindObjectOfType<EnvironmentSetup>();
        //rowCount = (int)Mathf.Sqrt (CarCount);
        maxDistanceX = environmentSetup.GetXStreetCenter(rowCount / 2); //(rowCount/2) * environmentSetup.BuildingDistanceX - environmentSetup.StreetWidth / 2 - environmentSetup.MainBuildingCenterX;
        maxDistanceY = environmentSetup.GetYStreetCenter(rowCount / 2);
        startPosX1 = -environmentSetup.GetXStreetCenter(0); // - 100;
        startPosX2 = environmentSetup.GetXStreetCenter(1);
        startPosY1 = -environmentSetup.GetYStreetCenter(0);
        startPosY2 = environmentSetup.GetYStreetCenter(1);

        posX = new float[] { startPosX2, -startPosX1, -maxDistanceX, -maxDistanceX, startPosX2, -startPosX1, maxDistanceX, maxDistanceX }; 
		posZ = new float[] { -maxDistanceY, -maxDistanceY, startPosY2, -startPosY1, maxDistanceY, maxDistanceY, startPosY2, -startPosY1 };
        angle = new float[] { 0, 0, 90, 90, 180, 180, 270, 270 };
        angleRad = new float[] { 0, 0, Mathf.PI / 2, Mathf.PI / 2, Mathf.PI, Mathf.PI, Mathf.PI / 2 * 3, Mathf.PI / 2 * 3 };


        Vector3 pos = new Vector3 ();

		for (int i = 0; i < posX.Length; i++) {
			pos.x = posX[i];
			pos.z = posZ[i];
			pos.y = - ( Random.Range (environmentSetup.MainBuildingHeight/2, environmentSetup.MainBuildingHeight/3*2) );

            if (i == 0)
            {
                CarParticleSystem.transform.position = pos;
                //CarParticleSystem.startRotation = angle[i];// * (180 / Mathf.PI);
            }
            else {
                ParticleSystem c = Instantiate(CarParticleSystem);
                c.transform.position = pos;
                c.transform.parent = CarParticleSystem.transform.parent;
                c.transform.Rotate(Vector3.up * angle[i]);
                float rot = angleRad[i];
                c.startRotation = rot;
                c.UpdateParticles(p =>
                {
                    p.rotation = angle[i];
                    return p;
                });
            }
		}
	}
}