using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Stage1_Logic : MonoBehaviour {
    public static event StageDone Stage1Done;
    public MaSineAsteroid[] asteroids;

	void Start () {
        //if (!isServer)
        //    return;
        asteroids = GetComponentsInChildren<MaSineAsteroid>();

        InvokeRepeating("CheckAsteroids", 0.23f, 0.23f);
	}

    private void CheckAsteroids() {
        int destroyed = 0;
        for (int i = 0; i < asteroids.Length; i++) {
            if (asteroids[i] == null)
                destroyed++;
        }

        // fire stage 1 done event
        if (destroyed == asteroids.Length) {
            CancelInvoke("CheckAsteroids");
            if (Stage1Done != null)
                Stage1Done();
        }
    }
}
