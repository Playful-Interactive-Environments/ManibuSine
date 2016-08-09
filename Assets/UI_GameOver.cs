﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour {
    private static UI_GameOver instance;
    public Text time, collected, shot, damage;

    public static void SetData(string time, int collected, int shot, int damage) {
        print("1blub");
        if (instance == null)
            return;
        print("2blub");
        instance.gameObject.SetActive(true);

        instance.time.text = time;
        instance.collected.text = collected.ToString();
        instance.shot.text = shot.ToString();
        instance.damage.text = damage.ToString();
    }

	void Awake () {
        instance = this;
        gameObject.SetActive(false);
	}

}