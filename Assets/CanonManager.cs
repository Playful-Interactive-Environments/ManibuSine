using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CanonManager : NetworkBehaviour {

    //[SyncVar]
    public GameObject canonPivot;

    public Transform gunner;
    private Transform gunnerHead;

    private Camera mainCamera;
    private Canon canon;

    void Start()
    {
        mainCamera = Camera.main;

        canon = canonPivot.GetComponentInChildren<Canon>();

        //InvokeRepeating("Shoot", 0.2f, 0.2f);
    }


    // PlayerAssigned Msg sent in cannon trigger
    void PlayerAssigned(Transform gunner)
    {
        this.gunner = gunner;
        this.gunnerHead = gunner.GetComponentInChildren<Head>().transform;
	}

    // PlayerGone Msg sent in cannon trigger
    void PlayerGone ()
    {
        gunner = null;
        gunnerHead = null;
	}

    void Shoot()
    {
        NetworkDataManager.Instance.CmdShoot();
    }

    void Update()
    {
        if (gunner != null)
        {
            canonPivot.transform.rotation = gunnerHead.rotation;
            canonPivot.transform.position = new Vector3(canonPivot.transform.position.x, canonPivot.transform.position.y, gunner.transform.position.z);
            if (gunner.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    print("canonMNGR: try shoot");
                    Shoot();
                }
            }
        }
    }
}
