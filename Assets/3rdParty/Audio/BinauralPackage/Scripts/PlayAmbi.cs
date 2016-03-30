using UnityEngine;
using System.Collections;
using TBE.Components;

public class PlayAmbi : MonoBehaviour {

	TBE_AmbiArray ambi;
	public AudioClip PeterWX;
	public AudioClip PeterYZ;
	public AudioClip FireworksWX;
	public AudioClip FireworksYZ;

	// Use this for initialization
	void Start () {

		ambi = GetComponent<TBE_AmbiArray>();

		ambi.clipWX = PeterWX;
		ambi.clipYZ = PeterYZ;
		ambi.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI ()
	{
		if (GUI.Button(new Rect(10, 10, 100, 50), "Peter Peter"))
		{	
			ambi.Stop();
			ambi.clipWX = PeterWX;
			ambi.clipYZ = PeterYZ;
			ambi.Play();
		}

		if (GUI.Button(new Rect(10, 60, 100, 50), "Fireworks"))
		{	
			ambi.Stop();
			ambi.clipWX = FireworksWX;
			ambi.clipYZ = FireworksYZ;
			ambi.Play();
		}
	}
}
