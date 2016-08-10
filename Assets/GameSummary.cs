using UnityEngine;
using System.Collections;
using System;
using System.Globalization;

public class GameSummary : MonoBehaviour {

    public static int ShotAsteroids;
    public static int CollectedItems;
    public static int HitByAsteroids;
    public static string PlayTime;

    private DateTime startTime;

    

	// Use this for initialization
	void Start () {
        ShipManager.GameOver += OnGameOver;
        Stage1_Logic.Stage1Done += OnStage1Done;
    }

    private void OnStage1Done() {
        StartGame();
    }

    public void StartGame() {
        print("started");
        startTime = DateTime.Now;
    }

    private void OnGameOver(int success) {
        // get time
        TimeSpan played = DateTime.Now.Subtract(startTime);
        PlayTime = played.Minutes + ":" + played.Seconds + ":" + played.Milliseconds;

        // get shot asteroids
        ShotAsteroids = MaSineAsteroid.destroyedAsteroids;
        // get collected items
        CollectedItems = PickUpRay.pickUpsInUpCargo;
        // get damage received
        HitByAsteroids = ShipManager.Instance.maxHP - ShipManager.Instance.currentHP;

        UI_GameOver.SetData(PlayTime, CollectedItems, ShotAsteroids, HitByAsteroids);
    }

    // Update is called once per frame
    void OnDestroy () {
        ShipManager.GameOver -= OnGameOver;
        Stage1_Logic.Stage1Done -= OnStage1Done;
    }
}
