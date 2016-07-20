using UnityEngine;
using System.Collections;


public enum MatAniStyle
{
    Down, UpDown
}
public class MaterialAnimator : MonoBehaviour {


    public MatAniStyle style = MatAniStyle.UpDown;
    public float animationSpeed = 2;

    public float minValue = -1.0f;
    public float maxValue = 1.0f;

    public Vector2 animationVector = Vector2.one;

    private Material mat;
    private float animationCurrent = 0;
    private bool animationUp = true;


	void Start () {
        mat = GetComponent<MeshRenderer>().material;
	}

	void Update () {
        if (style == MatAniStyle.Down)
        {
            AnimateDown();
        }
        else if (style == MatAniStyle.UpDown)
        {
            AnimateUpDown();
        }
        mat.mainTextureOffset = animationVector * animationCurrent;
	}

    private void AnimateDown()
    {
        if (animationUp)
        {
            if (animationCurrent < maxValue)
            {
                animationCurrent += Time.deltaTime * animationSpeed;
            }
            else
            {
                animationCurrent = minValue;
            }
        }
    }

    private void AnimateUpDown()
    {
        if (animationUp)
        {
            if (animationCurrent < maxValue)
            {
                animationCurrent += Time.deltaTime * animationSpeed;
            }
            else
            {
                animationUp = false;
            }
        }
        else
        {
            if (animationCurrent > minValue)
            {
                animationCurrent -= Time.deltaTime * animationSpeed;
            }
            else
            {
                animationUp = true;
            }
        }

        mat.mainTextureScale = animationVector * animationCurrent;
    }
}
