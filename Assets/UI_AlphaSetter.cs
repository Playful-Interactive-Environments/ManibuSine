using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_AlphaSetter : MonoBehaviour {
    [Header("Stets alpha values of all childeren.")]
    [Range(0, 1)]
    public float alpha = 0.5f;

	void Start () {
        MaskableGraphic[] graphics = GetComponentsInChildren<MaskableGraphic>();
        foreach (MaskableGraphic item in graphics) {
            Color c = item.color;
            item.color = new Color(c.r, c.g, c.b, alpha);
        } 
	}
}
