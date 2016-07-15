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
        small = 18,
        medium = 24,
        large = 28
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

    private void OnGameOver(int won)
    {
        IsRecieving = false;
        
        if (won > 0) // game won
        {
            DisplayText(DisplayArea.Middle, GameColor.Success, TextSize.large, "Your delivered the cargo.\nGame won!");
        }
        else // game lost
        {
            DisplayText(DisplayArea.Middle, GameColor.Alert, TextSize.large, "The cargo has been destroyed.\nGame lost!");
        }
    }

    public static void DisplayText(DisplayArea displayArea, Color color, TextSize textSize, string text)
    {
        foreach (UI_HeadUpTextField field in Instance.textFields)
        {
            if(field.areaName == displayArea)
            {
                field.DisplayText(color, textSize, text);
            }
        }
    }

    public static void DisplayText(DisplayArea displayArea, Color color, TextSize textSize, string text, float duration)
    {
        foreach (UI_HeadUpTextField field in Instance.textFields)
        {
            if (field.areaName == displayArea)
            {
                field.DisplayText(color, textSize, text, duration);
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