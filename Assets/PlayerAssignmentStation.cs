using UnityEngine;
using System.Collections;

public class PlayerAssignmentStation : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void MsgPlayerAssigned(Transform other)
    {
        Admin.Instance.CurrentTrackedPlayer = other.gameObject;
        if(Admin.Instance.PlayerOne == null)
        {
            Admin.Instance.ChoosePlayerOne();
        }
        else if(Admin.Instance.PlayerTwo == null)
        {
            Admin.Instance.ChoosePlayerTwo();
        }
    }


    void MsgPlayerGone(Transform other)
    {
        Admin.Instance.CurrentTrackedPlayer = null;
    }
}
