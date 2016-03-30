using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters;
public class AmbienceMove : MonoBehaviour
{

	public static AudioSource amb1, amb2, amb3, amb4, amb5;

	void Start () {
		AudioSource[] audios = GetComponents<AudioSource>();
		amb1 = audios[0];
		amb2 = audios[1];
		amb3 = audios[2];
		amb4 = audios[3];
		amb5 = audios[4];

	}
	
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		
	}
	
}
