using UnityEngine;
using System.Collections;

public class TextureOffset : MonoBehaviour {
	public float scrollSpeedX = 0.05F;
	public float scrollSpeedY = 0.01F;
	private Renderer rend;

	private float offsetX = 0;
	private float offsetY = 0;

	void Start() {
		rend = GetComponent<Renderer>();
	}
	void Update() {
		offsetX = Time.time * scrollSpeedX; // * Mathf.PerlinNoise(Time.time, 0.0F) / 255;
		offsetY = Time.time * scrollSpeedY; // * Mathf.PerlinNoise (0.0F, Time.time) / 255;

		if (rend.material.HasProperty("_BumpMap"))
			rend.material.SetTextureOffset("_BumpMap", new Vector2(offsetX, offsetY));

		if (rend.material.HasProperty("_MainTex"))
			rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));

		if (rend.material.HasProperty("_SpecMap"))
			rend.material.SetTextureOffset("_SpecMap", new Vector2(offsetX, offsetY));
		//rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);
	}
}
