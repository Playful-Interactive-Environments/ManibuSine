using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Ship : MonoBehaviour {

    private Image[] shipEnergyBars;



    void Start()
    {
        ShipCollider.ShipHit += OnShipHit;

        shipEnergyBars = GetComponentsInChildren<Image>();
    }

    private void OnShipHit()
    {
        // delay one frame
        StartCoroutine(OnShipHitDelayed());
    }

    private IEnumerator OnShipHitDelayed()
    {
        yield return 0;
        int toDeactivate = ShipManager.Instance.maxHP - ShipManager.Instance.currentHP;

        for (int i = 0; i < toDeactivate; i++)
        {
            if (i >= shipEnergyBars.Length)
                break;
            shipEnergyBars[i].enabled = false;
        }
    }

    void Dispose()
    {
        ShipCollider.ShipHit -= OnShipHit;
    }
}
