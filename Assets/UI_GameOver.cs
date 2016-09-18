using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour {
    private static UI_GameOver instance;
    public Text time, collected, shot, damage, score;

    public static void SetData(string time, int collected, int shot, int damage, int score) {
        if (instance == null)
            return;
        instance.gameObject.SetActive(true);

        instance.time.text = time;
        instance.collected.text = collected.ToString();
        instance.shot.text = shot.ToString();
        instance.damage.text = damage.ToString();
        instance.score.text = score.ToString();
    }

	void Awake () {
        instance = this;
	}

    void Start() {
        gameObject.SetActive(false);
    }
}
