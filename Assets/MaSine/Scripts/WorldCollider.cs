using UnityEngine;
using System.Collections;

public class WorldCollider : MonoBehaviour {

    public GameObject particle;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionStay(Collision collision)
    {
        //print("enter" + collision.contacts[0].thisCollider.GetComponent<MeshRenderer>());
        MeshRenderer mr = collision.contacts[0].thisCollider.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            //mr.enabled = true;
            particle.transform.position = collision.contacts[0].point;
            particle.GetComponent<ParticleSystem>().Play();
        }
            

            
    }

    void OnCollisionExit(Collision collision)
    {
        //print("exit");
        MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer meshRenderer in mr)
        {
            meshRenderer.enabled = false;
        }
    }
}
