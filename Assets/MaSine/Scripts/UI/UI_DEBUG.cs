using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_DEBUG : MonoBehaviour {
    private static Text text;
    private static int msgCount;
    private static int currentLines;
    private static int maxLines = 10;


    void Start()
    {
        text = GetComponentInChildren<Text>();
    }

    static void SetMaxLineNumber()
    {
        currentLines++;
        if (currentLines <= maxLines)
            return;

        string[] tempText = text.text.Split('\n');
        text.text = "";
        for (int i = 1; i < maxLines; i++)
        {
            text.text += tempText[i] + "\n";
        }
        currentLines--;
    }

    public static void AddText(string msg)
    {
        if (text == null)
            return;
        SetMaxLineNumber();

        text.text += ++msgCount + ". " + msg + "\n";
    }
}
