﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_HeadUpInfo : MonoBehaviour {

    public float distanceLimit = 10000f;
    public LayerMask whatToCollideWith;

    private Transform[] children;

    GameObject infoObject;
    Text head;
    Text description;

    // Use this for initialization
    void Start () {
        infoObject = GameObject.Find("UI_InfoLine");
        children = infoObject.GetComponentsInChildren<Transform>();
        head = infoObject.GetComponentsInChildren<Text>()[0];
        description = infoObject.GetComponentsInChildren<Text>()[1];

    }
	
	// Update is called once per frame
	void Update () {
        
        Ray ray = new Ray(transform.parent.parent.transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction* 100, Color.green, 0.1f);
        RaycastHit hit;
        
        
        if (Physics.Raycast(ray, out hit, distanceLimit, whatToCollideWith))
        {
            
            foreach(Transform obj in children)
            {
                obj.gameObject.SetActive(true);
            }

            if (hit.transform.gameObject.tag == "SteeringStation")
            {
                head.text = "steering station";
                description.text = "enter to take control over the ship.";
            }
            else if (hit.transform.gameObject.tag == "Asteroid")
            {
                head.text = "asteroid";
                description.text = "danger, asteroids can damage your cargo.";
            }
            else if (hit.transform.gameObject.tag == "CannonStation")
            {
                head.text = "canon station";
                description.text = "enter to take control over a board canon.";
            }
            else if (hit.transform.gameObject.tag == "PickUp")
            {
                head.text = "lost artefact";
                description.text = "an artefact from prior missions, drones help the navigator to collect them-";
            }

            infoObject.GetComponent<RectTransform>().transform.position = hit.transform.gameObject.transform.position;

        }
        else
        {
            foreach (Transform obj in children)
            {
                obj.gameObject.SetActive(false);
            }
        }
    }
}