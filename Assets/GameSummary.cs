using UnityEngine;
using System.Collections;
using System;
using System.Globalization;

public class GameSummary : MonoBehaviour {

    [Header("Score multiplicators")]
    public float xAstariods = 500;
    public float xCollected = 1000;
    public float xHit = -1000;
    public float xPlayTime = -6;
    public float xGameWon = 7500;

    public static bool GameIsOver;

    public static int ShotAsteroids;
    public static int CollectedItems;
    public static int HitByAsteroids;
    public static string PlayTime;

    public static int TotalScore;

    private DateTime startTime;

	// Use this for initialization
	void Start () {
        GameIsOver = false;
        ShipManager.GameOver += OnGameOver;
        Stage1_Logic.Stage1Done += OnStage1Done;

        StartGame();
    }

    private void OnStage1Done() {
        StartGame();
    }

    public void StartGame() {
        startTime = DateTime.Now;
    }

    private void OnGameOver(int success) {
        if (GameIsOver)
            return;

        GameIsOver = true;
        // get time
        TimeSpan played = DateTime.Now.Subtract(startTime);
        PlayTime = played.Minutes + ":" + played.Seconds + ":" + played.Milliseconds;
        // get shot asteroids
        ShotAsteroids = MaSineAsteroid.destroyedAsteroids;
        // get collected items
        CollectedItems = PickUpRay.pickUpsInUpCargo;
        // get damage received
        HitByAsteroids = ShipManager.Instance.maxHP - ShipManager.Instance.currentHP;

        // calculate total score
        TotalScore = (int)
            (ShotAsteroids * xAstariods +
            CollectedItems * xCollected + 
            HitByAsteroids * xHit +
            played.TotalSeconds * xPlayTime +
            success * xGameWon);


        UI_GameOver.SetData(PlayTime, CollectedItems, ShotAsteroids, HitByAsteroids, TotalScore);
    }

    // Update is called once per frame
    void OnDestroy () {
        ShipManager.GameOver -= OnGameOver;
        Stage1_Logic.Stage1Done -= OnStage1Done;
    }
}
