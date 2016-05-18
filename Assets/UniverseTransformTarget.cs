using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class UniverseTransformTarget : NetworkBehaviour
{

    // Use this for initialization
    //void OnStartClient()
    //{
    //    AssignTargetToUniverse();
    //}

    //void OnStartServer()
    //{
    //    AssignTargetToUniverse();
    //}

    void Start()
    {
        AssignTargetToUniverse();
    }

    private void AssignTargetToUniverse()
    {
        print("Start");
        UniverseTransformer.Instance.SetTargetTransform(transform);
    }
}