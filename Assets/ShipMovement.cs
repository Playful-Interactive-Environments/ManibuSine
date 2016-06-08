﻿using UnityEngine;
using System.Collections;

public class ShipMovement : MonoBehaviour {

    private static ShipMovement instance;
    public static ShipMovement Instance { get { return instance; } }

    public Transform targetTransform;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
	
	}

    // Use this for initialization
    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform == null)
            return;

        this.transform.position = targetTransform.position;
        this.transform.rotation = targetTransform.rotation;

    }

    public void MoveForward(float speed)
    {
        targetTransform.Translate(targetTransform.right * speed * Time.deltaTime, Space.World);
    }

    public void RotateRight(float rot)
    {
        targetTransform.Rotate(targetTransform.up, rot * Time.deltaTime);
    }
}