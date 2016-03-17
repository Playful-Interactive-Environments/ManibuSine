using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleHelper : MonoBehaviour {

    public Color[] startColor;

    private ParticleSystem ps;
    private ParticleCollisionEvent[] collisionEvents;
    private GameObject[] player;

    private float sqrStartSpeed;
    private List<Vector3> hitParticlePos;

    void Start() {
        ps = GetComponent<ParticleSystem>();
        collisionEvents = new ParticleCollisionEvent[16];
        sqrStartSpeed = ps.startSpeed * ps.startSpeed;

        player = GameObject.FindGameObjectsWithTag("NetworkPlayer");
        hitParticlePos = new List<Vector3>();
    }

	void FixedUpdate () {
        ps.UpdateParticles(p =>
        {
            Vector3 v = p.velocity;
            if (v.sqrMagnitude == sqrStartSpeed)
            {
                v = v * -1;
                p.velocity = v;
            }

            if (p.lifetime == p.startLifetime && startColor.Length > 0)
            {
                //Debug.Log(p.position);
                /*int index = (int)Random.Range(0, startColor.Length / 2);
                if (index >= startColor.Length / 2) index = startColor.Length / 2 - 1;
                p.color = Color.Lerp(startColor[index], startColor[index + startColor.Length / 2], Random.Range(0, 1.0F));*/
            }

            foreach (Vector3 pos in hitParticlePos)
            {
                Vector3 dir = pos - p.position;
                if (dir.sqrMagnitude < 2000)
                {
                    //p.color = Color.grey;
                    //p.velocity = dir * 100;

                    int index = 1;
                    p.color = Color.Lerp(startColor[index], startColor[index + startColor.Length / 2], Random.Range(0, 1.0F));
                }
            }

            return p;
        });

        hitParticlePos.Clear();


    }

    void OnParticleCollision(GameObject other)
    {
        //Debug.Log(LayerMask.LayerToName(other.layer));

        //TODO why is layer of player Default ????

        //if (other.layer == LayerMask.NameToLayer("player"))
        {
            int safeLength = ps.GetSafeCollisionEventSize();
            if (collisionEvents.Length < safeLength)
                collisionEvents = new ParticleCollisionEvent[safeLength];

            int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);

            //Debug.Log(numCollisionEvents);

            int i = 0;
            while (i < numCollisionEvents)
            {
                Vector3 pos = collisionEvents[i].intersection; //.colliderComponent.transform.position;
                hitParticlePos.Add(pos);

                

                //collisionEvents[i]
                //ps.SetParticles();
                i++;
            }
        }
    }
}
