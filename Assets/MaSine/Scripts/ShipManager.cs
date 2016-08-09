using UnityEngine;
using System.Collections;

public class ShipManager : MonoBehaviour {

    /// <summary>
    /// 0 means game lost
    /// > 0 means game won
    /// </summary>
    public static event ShipDelegate GameOver;

    private static ShipManager instance;
    public static ShipManager Instance
    {
        get { return instance; }
    }

    public int maxHP = 6;
    public int currentHP;
    public bool godMode;

    void Awake()
    {
        instance = this;
    }

    public void GameWon() {
        if (GameOver != null)
            GameOver(1);
    }
	// Use this for initialization
    public void Initialize()
    {
        currentHP = maxHP;
    }

    public void SetHP(int hp)
    {
        if (godMode)
            currentHP = maxHP;
        else
            currentHP = hp;

        if (currentHP <= 0)
            if (GameOver != null)
                if (!godMode)
                {
                    GameOver(0);
                    print("Game Is Over - And Lost");
                }
                    
    }
}
