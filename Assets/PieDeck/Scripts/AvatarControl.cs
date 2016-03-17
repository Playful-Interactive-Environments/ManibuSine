using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.VR;
using VRStandardAssets.Utils;
public class AvatarControl : MonoBehaviour {

    public GameObject PlayerAvatar;
    private bool hasGreen;
    private bool hasRed;
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (GameManager.Instance != null)
	    {
            if (GameManager.Instance.AvatarGreenTaken && !GameManager.Instance.AvatarGreenControlled && !hasGreen && !hasRed)
            {
                PlayerAvatar = GameManager.Instance.AvatarGreen;
                GameManager.Instance.Communicator.GetComponent<CommunicatorTest>().CmdGreenControlled();
                hasGreen = true;

            }
            if (GameManager.Instance.AvatarRedTaken && !GameManager.Instance.AvatarRedControlled && !hasRed && !hasGreen)
            {
                PlayerAvatar = GameManager.Instance.AvatarRed;
                GameManager.Instance.Communicator.GetComponent<CommunicatorTest>().CmdRedControlled();
                hasRed = true;
            }
	        if (hasGreen && !GameManager.Instance.AvatarGreenTaken)
	        {
	            hasGreen = false;
	        }
            if (hasRed && !GameManager.Instance.AvatarRedTaken)
            {
                hasRed = false;
            }

            if (PlayerAvatar != null)
            {
                if (Input.GetButtonDown("Cancel"))
                {
                    ServerManager.Instance.ReconnectClient();
                }
                this.transform.position = PlayerAvatar.transform.position;
            }
        }
           
    }
}
