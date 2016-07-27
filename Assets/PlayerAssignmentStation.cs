using UnityEngine;
using System.Collections;

public class PlayerAssignmentStation : MonoBehaviour {
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Admin.Instance.CurrentTrackedPlayer = other.gameObject;
            if (Admin.Instance.PlayerOne == null)
            {
                Admin.Instance.ChoosePlayerOne();
            }
            else if (Admin.Instance.PlayerTwo == null)
            {
                Admin.Instance.ChoosePlayerTwo();
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
