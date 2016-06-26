using UnityEngine;
using System.Collections;

public class ShipManager : MonoBehaviour {

    private static ShipManager instance;
    public static ShipManager Instance
    {
        get { return instance; }
    }

    public int maxHP = 6;
    public int currentHP; // sync that

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        currentHP = maxHP; // sync that!!!
        ShipCollider.ShipHit += OnShipHit;
	}

    public void Initialize()
    {
        currentHP = maxHP;
    }

    private void OnShipHit()
    {
        currentHP--;
    }

    void Dispose()
    {
        ShipCollider.ShipHit -= OnShipHit;
    }
}
