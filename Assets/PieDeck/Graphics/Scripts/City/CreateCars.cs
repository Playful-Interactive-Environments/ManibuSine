using UnityEngine;
using System.Collections;

public class CreateCars : MonoBehaviour {

	public GameObject MainCar;
	public GameObject CarParticleSystem;
	public int CarCount = 100;

	private int rowCount;
	private float maxDistance;
	private float startPos;

	private float[] posX; 
	private float[] posZ; 
	private float[] angle; 
	private int[] dir = { 1, 1, 3, 3, 0, 0, 2, 2 }; 

	void Start() {
		rowCount = (int)Mathf.Sqrt (CarCount);
		maxDistance = (rowCount/2) * 200 + 100;
		startPos = maxDistance - 100;

		posX = new float[] { 100, -100, -startPos, -startPos, 100, -100, startPos, startPos }; 
		posZ = new float[] { -startPos, -startPos, 100, -100, startPos, startPos, 100, -100 }; 
		angle = new float[] { 0, 0, 90, 90, 180, 180, 270, 270 }; 

		Vector3 pos = new Vector3 ();

		for (int i = 0; i < posX.Length; i++) {
			pos.x = posX[i];
			pos.z = posZ[i];
			pos.y = - ( Random.Range (50, 150) );

			if (i == 0)
				CarParticleSystem.transform.position = pos;
			else {
				GameObject c = Instantiate (CarParticleSystem);
				c.transform.position = pos;
				c.transform.parent = CarParticleSystem.transform.parent;
				c.transform.Rotate (Vector3.up * angle[i]);
			}
		}
	}

	void SingleBubbleStart () {


		for (int i = 0; i < rowCount; i++) {

			float posValue = (i - rowCount/2) * 200 + 100;
			Vector3 pos = new Vector3 ();

			pos.x = 100;
			pos.z = posValue;
			pos.y = - ( Random.Range (0, 200) );
			creatCar (pos, (int)Random.Range(0, 1.999999999F));

			pos.x = -100;
			pos.z = posValue;
			pos.y = - ( Random.Range (0, 200) );
			creatCar (pos, (int)Random.Range(0, 1.999999999F));

			pos.z = 100;
			pos.x = posValue;
			pos.y = - ( Random.Range (0, 200) );
			creatCar (pos, (int)Random.Range(2, 3.999999999F));

			pos.z = -100;
			pos.x = posValue;
			pos.y = - ( Random.Range (0, 200) );
			creatCar (pos, (int)Random.Range(2, 3.999999999F));
		}
	}


	void SingleBubbleFixedUpdate() {

		if ((int)Random.Range (0, 50) != 1)
			return;

		Vector3 pos = new Vector3 ();

		for (int i = 0; i < posX.Length; i++) {
			pos.x = posX[i];
			pos.z = posZ[i];
			pos.y = - ( Random.Range (0, 200) );
			creatCar (pos, dir[i]);
		}
	}

	void creatCar (Vector3 pos, int dir) {
		Vector3 scale = new Vector3 ();
		scale.x = Random.Range (4, 18) * 10;
		scale.y = Random.Range (350, 650);
		scale.z = Random.Range (4, 12) * 10;

		GameObject b = Instantiate (MainCar);
		b.SetActive (true);
		b.transform.position = pos;
		//b.transform.localScale = scale;
		b.transform.parent = MainCar.transform.parent;

		AutoMoveCarComponent mc = b.GetComponent<AutoMoveCarComponent> ();
		int randDir = dir; 
		switch (randDir) {
		case 0:
			mc.direction = Vector3.forward;
			break;
		case 1:
			mc.direction = Vector3.back;
			break;
		case 2:
			mc.direction = Vector3.right;
			break;
		case 3:
			mc.direction = Vector3.left;
			break;

		}

		mc.speed = Random.Range (2, 10);
		mc.maxDistance = maxDistance;
	}
}
