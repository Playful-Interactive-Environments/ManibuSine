using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UI_Ship : MonoBehaviour {
    private static UI_Ship instance;
    public static UI_Ship Instance
    {
        get { return instance; }
    }

    private Image[] shipEnergyBars;
    public Text destroyed;
    public Text pickedUp;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        shipEnergyBars = GetComponentsInChildren<Image>();
    }

    public void SetPickedUp(int picked)
    {
        pickedUp.text = picked.ToString();
    }

    public void SetHP(int damage)
    {
        int toDeactivate = ShipManager.Instance.maxHP - ShipManager.Instance.currentHP;
        for (int i = 0; i < toDeactivate; i++)
        {
            if (i >= shipEnergyBars.Length)
                break;
            shipEnergyBars[i].enabled = false;
        }

        if (damage <= 0)
            destroyed.enabled = true;
    }
}
