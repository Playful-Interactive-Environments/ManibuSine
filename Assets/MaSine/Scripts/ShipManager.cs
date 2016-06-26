using UnityEngine;
using System.Collections;

public class ShipManager : MonoBehaviour {

    private static ShipManager instance;
    public static ShipManager Instance
    {
        get { return instance; }
    }

    public int maxHP = 6;
    public int currentHP;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
    public void Initialize()
    {
        currentHP = maxHP;
    }

    public void SetHP(int hp)
    {
        currentHP = hp;
    }
}
