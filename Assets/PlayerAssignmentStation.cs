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
                    return;
                }
                
            }
            if (Admin.Instance.PlayerTwo != null)
            {
                if (Admin.Instance.PlayerTwo.GetComponent<NetworkPlayer>().ControllingPlayer == null)
                {
                    Admin.Instance.ChoosePlayerTwo();
                    return;
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
