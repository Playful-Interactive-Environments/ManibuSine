using UnityEngine;
using System.Collections;

public class GameColor : MonoBehaviour {

    private static GameColor instance;

    public Color neutral;
    public Color alert;
    public Color success;

    void Awake()
    {
        instance = this;
    }

    public static Color Neutral
    {
        get
        {
            return instance.neutral;
        }

        set
        {
            instance.neutral = value;
        }
    }

    public static Color Alert
    {
        get
        {
            return instance.alert;
        }

        set
        {
            instance.alert = value;
        }
    }

    public static Color Success
    {
        get
        {
            return instance.success;
        }

        set
        {
            instance.success = value;
        }
    }
}
