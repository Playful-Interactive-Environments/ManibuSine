using UnityEngine;
using System.Collections;


public enum MatAniStyle
{
    Down, UpDown
}
public class MaterialAnimator : MonoBehaviour {


    public MatAniStyle style = MatAniStyle.UpDown;
    public float animationSpeed = 2;

    private Material mat;
    private float animationDistance = 2.5f;
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
        mat.mainTextureScale = Vector2.one * animationCurrent;
	}

    private void AnimateDown()
    {
        if (animationUp)
        {
            if (animationCurrent < animationDistance)
            {
                animationCurrent += Time.deltaTime * ((animationCurrent + 0.1f) * animationSpeed);
            }
            else
            {
                animationCurrent = 0.4f;
            }
        }
    }

    private void AnimateUpDown()
    {
        if (animationUp)
        {
            if (animationCurrent < animationDistance)
            {
                animationCurrent += Time.deltaTime * ((animationCurrent + 0.1f) * animationSpeed);
            }
            else
            {
                animationUp = false;
            }
        }
        else
        {
            if (animationCurrent > 0.4f)
            {
                animationCurrent -= Time.deltaTime * ((animationCurrent + 0.1f) * animationSpeed);
            }
            else
            {
                animationUp = true;
            }
        }

        mat.mainTextureScale = Vector2.one * animationCurrent;
    }
}
