using UnityEngine;
using System.Collections;

public class maxLifetimeComponent : MonoBehaviour {

	private int age = 0;
	private int maxAge = 500;

	void FixedUpdate() {
		age++;

		if (age >= maxAge)
			Destroy (gameObject);
	}
}
