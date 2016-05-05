using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    public List<AudioClip> clips;

    public GameObject prefab;

    void Awake()
    {
        instance = this;
    }
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
        AudioSource aSource = tempGO.GetComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip
        // set other aSource properties here, if desired
        aSource.Play(); // start the sound
        Destroy(tempGO, clip.length); // destroy object after clip duration
    }
}
