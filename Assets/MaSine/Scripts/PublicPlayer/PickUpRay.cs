using UnityEngine;
using System.Collections;

public delegate void PickUpDelegateInt(int picked);
public delegate void PickUpDelegate(int picked, PickUpRay ray);

public class PickUpRay : MonoBehaviour {
    public static PickUpDelegateInt PickedItem, LostTarget;
    public static PickUpDelegate GotTarget;


    private Head navigatorHead;
    public int playerID;
    private SteeringStation steeringStation;

    public PublicPickUp pickUp;

    public static int pickUpsInUpCargo = 0;

    private float pickUpDuration = 2.0f;
    private float currentPickUpTime = 999;
    private float pickUpProgress01 = 0;
    public float PickUpProgress01
    {
        get { return pickUpProgress01; }
    }

	void Start () {
        steeringStation = GetComponentInParent<SteeringStation>();

        SteeringStation.EnteredSteering += OnEnteredSteering;
        SteeringStation.ExitedSteering += OnExitedSteering;
	}

    private bool hadTarget;
    void Update()
    {
        if (navigatorHead == null)
            return;
        if (navigatorHead.pickUp == null)
        {
            if (hadTarget)
            {
                hadTarget = false;
                if (LostTarget != null)
                    LostTarget(playerID);
            }

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

            hadTarget = true;
            currentPickUpTime = 0;
            pickUpProgress01 = 0;

            if (GotTarget != null)
                GotTarget(playerID, this);
        }
        else
        {
            if (currentPickUpTime < pickUpDuration)
            {
                currentPickUpTime += Time.deltaTime;
                pickUpProgress01 = Mathf.Clamp01(currentPickUpTime / pickUpDuration);

                if (currentPickUpTime >= pickUpDuration)
                {
                    pickUpsInUpCargo++;
                    if (PickedItem != null)
                        PickedItem(pickUpsInUpCargo);

                    if (LostTarget != null)
                        LostTarget(playerID);
                    
                    steeringStation.NetworkPlayer.CmdPickItUp(pickUp.netId.Value);
                }
            }
        }
    }
    

    private void OnEnteredSteering(SteeringStation steeringStation)
    {
        if (steeringStation.navigator == null)
            return;

        navigatorHead = steeringStation.navigator.GetComponentInChildren<Head>();
        playerID = steeringStation.navigator.gameObject.GetInstanceID();

    }
    private void OnExitedSteering(SteeringStation steeringStation)
    {
        navigatorHead = null;
        if (LostTarget != null)
            LostTarget(playerID);
        
        playerID = 0;
    }


    void OnDestroy()
    {
        // causes null reference
        SteeringStation.EnteredSteering -= OnEnteredSteering;
        SteeringStation.ExitedSteering -= OnExitedSteering;
    }
}
