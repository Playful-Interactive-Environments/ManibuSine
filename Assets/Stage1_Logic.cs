using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Stage1_Logic : MonoBehaviour {
    private static Stage1_Logic instance;

    public static event StageDone Stage1Done;
    public GameObject asteroidPrefab;
    public MaSineAsteroid[] asteroids;

    void Awake() {
        instance = this;
    }

	public static void SpawnStage1 () {
        // collect spawn positions
        SpawnPosition[] spawnPos = instance.GetComponentsInChildren<SpawnPosition>();

        foreach (SpawnPosition item in spawnPos) {
            // instantiate object
            GameObject newObj = ServerManager.Instance.SpawnEntityAt(instance.asteroidPrefab, item.transform.position, Quaternion.identity);
            MaSineAsteroid newAsteroid = newObj.GetComponent<MaSineAsteroid>();
            if (newAsteroid == null)
                return;

            // parent stage one asteroids to this
            if (item.isStageOneAsteroid)
                newObj.transform.parent = instance.transform;
            else // others to universe
                newObj.transform.parent = UniverseTransformer.Instance.transform;
            // set asteroid as static
            newAsteroid.isStatic = true;
        }

        // collect static asteroids
        instance.asteroids = instance.GetComponentsInChildren<MaSineAsteroid>();

        // delete all spawn position game objects
        for (int i = 0; i < spawnPos.Length; i++)
            Destroy(spawnPos[i].gameObject);
        spawnPos = null;

        instance.InvokeRepeating("CheckAsteroids", 0.23f, 0.23f);
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
