using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject BallPrefab;
	private int _ballsAmount;

	void Start () {
		InvokeRepeating("SpawnBall", 1.0f, 0.1f);
		
	}
	
	// Update is called once per frame
	void Update () {
		if(_ballsAmount > 200)
		{
			CancelInvoke("SpawnBall");
		}
	}
	void SpawnBall()
	{
		
		Instantiate(BallPrefab, transform.position, Quaternion.identity);
		_ballsAmount++;
		
	}
}
