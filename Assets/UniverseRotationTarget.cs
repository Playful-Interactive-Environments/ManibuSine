using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class UniverseRotationTarget : NetworkBehaviour
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
        UniverseTransformer.Instance.SetTargetRotator(transform);
    }
}