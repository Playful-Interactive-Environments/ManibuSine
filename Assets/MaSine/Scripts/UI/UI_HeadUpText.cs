using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_HeadUpText : MonoBehaviour {
    private static UI_HeadUpText instance;
    UI_HeadUpTextField[] textFields;

    private bool isRecieving;


    public static UI_HeadUpText Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }
    public bool IsRecieving
    {
        get
        {
            return isRecieving;
        }

        set
        {
            isRecieving = value;
        }
    }

    public enum DisplayArea
    {
        Middle,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public enum TextSize
    {
        small = 10,
        medium = 12,
        large = 18
    }

    void Awake()
    {
        Instance = this;
    }

	void Start () {
        ShipManager.GameOver += OnGameOver;
        textFields = GetComponentsInChildren<UI_HeadUpTextField>();
        IsRecieving = true;
	}

    string RandomString(int length) {
        string chars = "abcdefghijklmnopqrstuvwxyz.,:;-_#'+*!§$%&/()=?1234567890ß";
        string returnString = "";
        char[] arr = chars.ToCharArray();
        for (int i = 0; i < length; i++)
        {
            returnString += arr[Random.Range(0, chars.Length)];
        }
        return returnString;
    }

    private void OnGameOver(int won)
    {
        IsRecieving = false;
        
        if (won > 0) // game won
        {
            DisplayText(DisplayArea.Middle, GameColor.Success, TextSize.large, "your delivered the cargo.\ngame won!", 2);
            DisplayText(DisplayArea.BottomLeft, GameColor.Neutral, TextSize.medium, "delivery code: " + RandomString(Random.Range(3, 6)), 2);
        }
        else // game lost
        {
            //DisplayText(DisplayArea.Middle, GameColor.Alert, TextSize.large, "the cargo has been destroyed.\ngame lost!", 2);
            DisplayText(DisplayArea.BottomLeft, GameColor.Alert, TextSize.medium, "error code: " + RandomString(Random.Range(3, 6)) + "the cargo has been destroyed.\ngame lost!", 2);
        }
    }

    public static void DisplayText(DisplayArea displayArea, Color color, TextSize textSize, string text)
    {
        if (Instance != null)
        {
            foreach (UI_HeadUpTextField field in Instance.textFields)
            {
                if (field.areaName == displayArea)
                {
                    field.DisplayText(color, textSize, text);
                }
            }
        }
    }

    public static void DisplayText(DisplayArea displayArea, Color color, TextSize textSize, string text, float duration)
    {
        if(Instance != null)
        {
            foreach (UI_HeadUpTextField field in Instance.textFields)
            {
                if (field.areaName == displayArea && field != null)
                {
                    field.DisplayText(color, textSize, text, duration);
                }
            }
        }
        
    }

    //public static void ShowTextOnHeadUp(string msg, float duration)
    //{
    //    text.fontSize = smallText;
    //    text.text = msg;
    //    instance.ClearText(duration);
    //}
    //private void ClearText(float duration)
    //{
    //    text.color = originalColor;
    //    StartCoroutine(ClearTextRoutine(duration));
    //}
    //IEnumerator ClearTextRoutine(float duration)
    //{
    //    yield return new WaitForSeconds(duration);
    //    text.text = "";
    //}

    void Dispose () {
        ShipManager.GameOver -= OnGameOver;
	}
}