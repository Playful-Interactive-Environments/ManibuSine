using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipManager : MonoBehaviour {

    /// <summary>
    /// 0 means game lost
    /// > 0 means game won
    /// </summary>
    public static event ShipDelegate GameOver;
    public List<GameObject> objectsToDisable;

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
        GameOver += OnGameOver;
        objectsToDisable = new List<GameObject>();
    }

	// Use this for initialization
    public void Initialize()
    {
        currentHP = maxHP;
    }

    public void SetHP(int hp)
    {
        currentHP = hp;

        if (currentHP <= 0)
            if (GameOver != null)
                GameOver(0);
    }

    public void OnGameOver(int i)
    {
        foreach(GameObject go in objectsToDisable)
        {
            go.SetActive(false);
        }
    }
}
