using UnityEngine;
using System.Collections;

public class AudioSourceObject : MonoBehaviour {

    public GameObject prefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public void PlayClipAt(AudioClip clip, Vector3 pos) 
   {
       GameObject tempGO = Instantiate(prefab); // create the temp object
       tempGO.transform.position = pos; // set its position
       AudioSource aSource = GetComponent<AudioSource>(); // add an audio source
       aSource.clip = clip; // define the clip
       // set other aSource properties here, if desired
       aSource.Play(); // start the sound
       Destroy(tempGO, clip.length); // destroy object after clip duration
 }
}
