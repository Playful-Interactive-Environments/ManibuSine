using UnityEngine;
using System.Collections;

public class PickUpRay : MonoBehaviour {
    private Head navigatorHead;
    private SteeringStation steeringStation;

    private PublicPickUp pickUp;

    public int pickUpsInUpCargo = 0;

    private float pickUpDuration = 2.0f;
    private float currentPickUpTime = 0;

	void Start () {
        steeringStation = GetComponentInParent<SteeringStation>();

        steeringStation.EnteredSteering += OnEnteredSteering;
        steeringStation.ExitedSteering += OnExitedSteering;
	}

    void Update()
    {
        if (navigatorHead == null)
            return;
        if (navigatorHead.pickUp == null)
        {
            pickUp = null;
            return;
        }

        // start with pick up process
        if (pickUp == null)
        {
            pickUp = navigatorHead.pickUp.GetComponent<PublicPickUp>();
            if (pickUp == null)
                return;
            // not carried by a player
            if (pickUp.Player == null)
                return;

            currentPickUpTime = 0;
        }
        else
        {
            if (currentPickUpTime < pickUpDuration)
            {
                currentPickUpTime += Time.deltaTime;
            }
            else
            {
                pickUpsInUpCargo++;

                steeringStation.NetworkPlayer.CmdDestroyEntity(pickUp.gameObject);
            }
        }




    }

    private void OnEnteredSteering(SteeringStation steeringStation)
    {
        if (steeringStation.navigator != null)
            navigatorHead = steeringStation.navigator.GetComponentInChildren<Head>();

    }
    private void OnExitedSteering(SteeringStation steeringStation)
    {
        navigatorHead = null;
    }


    void OnDestroy()
    {
        // causes null reference
        //steeringStation.EnteredSteering -= OnEnteredSteering;
        //steeringStation.ExitedSteering -= OnExitedSteering;
    }
}
