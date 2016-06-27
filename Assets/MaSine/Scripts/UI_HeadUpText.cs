using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_HeadUpText : MonoBehaviour {
    private static UI_HeadUpText instance;
    private static Text text;

    private static int smallText = 18;
    private static int mediumText = 24;
    private static int bigText = 28;

    private Color originalColor;
    public Color wonColor;
    public Color lostColor;

    void Awake()
    {
        instance = this;
    }

	void Start () {
        ShipManager.GameOver += OnGameOver;
        text = GetComponentInChildren<Text>();
        originalColor = text.color;
	}

    private void OnGameOver(int won)
    {
        text.fontSize = mediumText;
        if (won > 0) // game won
        {
            text.color = wonColor;
            text.text = "Your delivered the cargo.\nGame won!";
        }
        else // game lost
        {
            text.color = lostColor;
            text.text = "The cargo has been destroyed.\nGame lost!";
        }
    }

    public static void ShowTextOnHeadUp(string msg, float duration)
    {
        text.fontSize = smallText;
        text.text = msg;
        instance.ClearText(duration);
    }
    private void ClearText(float duration)
    {
        text.color = originalColor;
        StartCoroutine(ClearTextRoutine(duration));
    }
    IEnumerator ClearTextRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        text.text = "";
    }

	void Dispose () {
        ShipManager.GameOver -= OnGameOver;
	}
}