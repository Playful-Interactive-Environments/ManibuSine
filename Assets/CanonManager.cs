using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CanonManager : NetworkBehaviour {

    [SyncVar]
    public GameObject canon;

    public Transform gunner;

    // PlayerAssigned Msg sent in cannon trigger
    void PlayerAssigned(Transform gunner)
    {
        this.gunner = gunner;
	}

    // PlayerGone Msg sent in cannon trigger
    void PlayerGone ()
    {
        gunner = null;
	}

    void Update()
    {
        if (gunner != null)
        {
            print("update " + gunner.transform.position);
            canon.transform.position = new Vector3(canon.transform.position.x, canon.transform.position.y, gunner.transform.position.z);
        }
    }
}
