using UnityEngine;
using System.Collections;

public class PlayerAssignmentStation : MonoBehaviour {
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Admin.Instance.CurrentTrackedPlayer = other.gameObject;
            if (Admin.Instance.PlayerOne != null)
            {
                if(Admin.Instance.PlayerOne.GetComponent<NetworkPlayer>().ControllingPlayer == null)
                {
                    Admin.Instance.ChoosePlayerOne();
                    Admin.Instance.RecalibratePlayer();
                    return;
                }
            }
            if (Admin.Instance.PlayerTwo != null)
            {
                if (Admin.Instance.PlayerTwo.GetComponent<NetworkPlayer>().ControllingPlayer == null)
                {
                    if (Admin.Instance.CurrentTrackedPlayer != Admin.Instance.PlayerOne.GetComponent<NetworkPlayer>().ControllingPlayer)
                    {
                        Admin.Instance.ChoosePlayerTwo();
                        Admin.Instance.RecalibratePlayer();
                        return;
                    }
                }
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Admin.Instance.CurrentTrackedPlayer = null;
        }
    }
}
