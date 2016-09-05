using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TrackedPlayerNetworkBehaviour : NetworkBehaviour {

	[SyncVar]
	public bool HasPlayerOne;
	[SyncVar]
	public bool HasPlayerTwo;

	public GameObject ControlledPlayer;

    public float distanceLimit = 10000f;
    public LayerMask whatToCollideWith;

    void Start () {

	}


    void Update()
    {
        if (ServerManager.Instance.isServer)
        {
            if (NetworkServer.active) {
                NetworkServer.Spawn(gameObject);
                this.transform.FindChild("Server").gameObject.SetActive(true);
                this.transform.FindChild("Client").gameObject.SetActive(false);
            }
        }
        if (ServerManager.Instance.isClient)
        {
            this.transform.FindChild("Server").gameObject.SetActive(false);
            if (HasPlayerOne || HasPlayerTwo)
            {
                this.transform.FindChild("Client").gameObject.SetActive(false);
            }
            if (!HasPlayerOne && !HasPlayerTwo)
            {
                this.transform.FindChild("Client").gameObject.SetActive(true);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distanceLimit, whatToCollideWith))
            {
                if (hit.transform.gameObject != this.gameObject)
                    return;

                Admin.Instance.CurrentTrackedPlayer = hit.transform.gameObject;
                StartCoroutine("ChangeColor");
            }
        }
    }
	void OnDestroy()
	{
		if (ServerManager.Instance.isServer)
		{
            if (ControlledPlayer != null)
            {
                // Todo reset networkplayer position
                print("tracked player gone");

                if (HasPlayerOne)
                {
                    Admin.Instance.ButtonPlayerOne.interactable = true;
                }
                if (HasPlayerTwo)
                {
                    Admin.Instance.ButtonPlayerTwo.interactable = true;

                }
            }
            
		}
	}
	void OnMouseDown()
	{
            //Admin.Instance.CurrentTrackedPlayer = gameObject;
			//StartCoroutine("ChangeColor");
	}

	IEnumerator ChangeColor()
	{
		Color current = this.transform.FindChild("Server").GetComponent<MeshRenderer>().material.color;
		this.transform.FindChild("Server").GetComponent<MeshRenderer>().material.color = Color.black;
		yield return new WaitForSeconds(2f);
		this.transform.FindChild("Server").GetComponent<MeshRenderer>().material.color = current;

	}


}