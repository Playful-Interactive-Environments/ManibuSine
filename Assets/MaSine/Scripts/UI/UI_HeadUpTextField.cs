using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_HeadUpTextField : MonoBehaviour {

    private Text text;
    public UI_HeadUpText.DisplayArea areaName ;
    public string textStartSign;

    private IEnumerator currentCoroutine;


    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
        text.text = textStartSign;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisplayText(Color color, UI_HeadUpText.TextSize textSize, string text)
    {
        this.text.fontSize = (int)textSize;
        this.text.color = color;
        this.text.text = textStartSign + text;
    }

    public void DisplayText(Color color, UI_HeadUpText.TextSize textSize, string text, float duration)
    {
        DisplayText(color, textSize, text);
        ClearText(duration);
    }

    private void ClearText(float duration)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        
        currentCoroutine = ClearTextRoutine(duration);
        StartCoroutine(currentCoroutine);
    }
    IEnumerator ClearTextRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        text.color = GameColor.Neutral;
        text.text = textStartSign;
    }
}
