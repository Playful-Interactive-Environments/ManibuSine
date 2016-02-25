using UnityEngine;
using System.Collections;
using VRStandardAssets.Common;
using VRStandardAssets.Utils;

public class RobotControl : MonoBehaviour {
	
	public AudioClip[] dialogue;
	private GameObject dummyAudioObject;
	private Vector3 newPosition;
	public VRInteractiveItem m_InteractiveItem;
	public GameObject damageParticles;
	public GameObject destroyParticles;
	private int positionCounter;
	public AudioSource damageSound;
	public AudioSource destroySound;

	public float speed = 0.5f;
	private Vector3 centerPoint = new Vector3 (960,150,540);
	private float spawnRadius = 500;
	private float randomValue;
	private int damage;

	void Start () {
		dummyAudioObject = new GameObject ();
		dummyAudioObject.AddComponent<AudioSource> ();
		dummyAudioObject.AddComponent<ReadAmplitude> ();
		dummyAudioObject.GetComponent<AudioSource>().bypassReverbZones = true;
		dummyAudioObject.GetComponent<AudioSource>().spatialBlend = 0;

		newPosition = transform.position;

		InvokeRepeating ("playRandomDia", 1, 7);
		InvokeRepeating ("moveRobot", 1, 10);
	}

	// Update is called once per frame
	void Update () {
		if(damage < 5)
		{
			transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
		}
		
	   if(damage >= 5)
		{
			Instantiate(destroyParticles, this.transform.position, Quaternion.identity);
			//destroySound.Play();
			this.transform.position = RandomCircle(centerPoint, spawnRadius);
			newPosition = this.transform.position;
			positionCounter = 0;
			damage = 0;
		   
		}
	}
	void OnEnable()
	{
		m_InteractiveItem.OnDown += HandleClick;
	}
	private void HandleClick()
	{
		Instantiate(damageParticles, this.transform.position, Quaternion.identity);
		//damageSound.Play();
		damage += 1;
		//soundPlayed;
	}

	void playRandomDia() {
		int i = Random.Range (0, 10);
		gameObject.GetComponent<AudioSource> ().PlayOneShot (dialogue [i]);
		dummyAudioObject.GetComponent<AudioSource>().PlayOneShot (dialogue[i]);
	}

	void moveRobot() {

		positionCounter++;
		float randomAddition = Random.Range (0.01f, 0.09f);
		switch (positionCounter) {
		case 1:
			newPosition = RandomCircle(centerPoint, spawnRadius);
			break;
		case 2:
			newPosition = new Vector3(900f, 250f, 800f);
			break;
		case 3:
			newPosition = new Vector3(RandomTarget(200f, 500f), 400f, RandomTarget(600f, 900f));
			break;
		case 4:
			newPosition = RandomCircle(centerPoint, spawnRadius);
				break;
		case 5:
			newPosition = new Vector3(900f, 150f, 700f);
			break;
		}
		if (positionCounter == 4)
			positionCounter = 0;
	}
	private float RandomTarget(float a, float b)
	{
		return Random.Range(a, b);
	}
	public static Vector3 RandomCircle(Vector3 theCenter, float theRadius)
	{
		float anAngle = Random.value * 360;
		Vector3 aPosOnCircle = new Vector3(
			theCenter.x + theRadius * Mathf.Sin(anAngle * Mathf.Deg2Rad),
			theCenter.y,
			theCenter.z + theRadius * Mathf.Cos(anAngle * Mathf.Deg2Rad)
			);
		return aPosOnCircle;
	}


}
