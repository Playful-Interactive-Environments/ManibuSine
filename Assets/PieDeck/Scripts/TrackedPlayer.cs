using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TrackedPlayer : ATuioPlayer
{
	public float Height = 50.0f;
	bool _isNetworkPlayer;
	private bool _hasRedPlayer;
	private bool _hasGreenPlayer;

	public override void MoveTo(Vector2 coords)
	{
		this.transform.position = new Vector3(coords.x, Height, coords.y);

	}
	void Start () {
		if(ServerManager.Instance.isNetworkActive){
			this.transform.FindChild("Body").gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerStay(Collider other) {
		if(other.transform.name == "Red" && ServerManager.Instance.isNetworkActive)
		{
			if (!_isNetworkPlayer && !GameManager.Instance.AvatarRedTaken)
			{

				GameManager.Instance.AvatarRed.GetComponent<AvatarPlayer>().setActive(true, this.transform.gameObject);
				GetComponent<CharacterController>().enabled = false;
				GameManager.Instance.AvatarRedTaken = true;
				_isNetworkPlayer = true;
				_hasRedPlayer = true;

			}
			else return;
			
		}
		if (other.transform.name == "Green" && ServerManager.Instance.isNetworkActive)
		{
			if (!_isNetworkPlayer && !GameManager.Instance.AvatarGreenTaken)
			{

				GameManager.Instance.AvatarGreen.GetComponent<AvatarPlayer>().setActive(true, this.transform.gameObject);
				GetComponent<CharacterController>().enabled = false;
				GameManager.Instance.AvatarGreenTaken = true;
				_isNetworkPlayer = true;
				_hasGreenPlayer = true;

			}
			else return;

		}
	}
	void OnDestroy()
	{
		if (_hasRedPlayer)
		{
			GameManager.Instance.AvatarRedTaken = false;
			GameManager.Instance.AvatarRedControlled = false;
			GameManager.Instance.AvatarRed.GetComponent<AvatarPlayer>().setActive(false, null);
		}
		if(_hasGreenPlayer)

		{
			GameManager.Instance.AvatarGreenTaken = false;
			GameManager.Instance.AvatarGreenControlled = false;
			GameManager.Instance.AvatarGreen.GetComponent<AvatarPlayer>().setActive(false, null);
		}

	}
}
