using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct ParticleHit
{
    public Vector3 Position;
    public int ColorId;

    public ParticleHit(Vector3 pos, int colorId)
    {
        Position = pos;
        ColorId = colorId;
    }
}

public class ParticleHelper : MonoBehaviour {

    private ParticleSystem ps;
    private ParticleCollisionEvent[] collisionEvents;
    private GameObject[] player;

    private float sqrStartSpeed;
    private List<ParticleHit> hitParticlePos;

    private EnvironmentSetup environmentSetup;

    void Start() {
        environmentSetup = GameObject.FindObjectOfType<EnvironmentSetup>();
        ps = GetComponent<ParticleSystem>();
        collisionEvents = new ParticleCollisionEvent[16];
        sqrStartSpeed = ps.startSpeed * ps.startSpeed;

        player = GameObject.FindGameObjectsWithTag("NetworkPlayer");
        hitParticlePos = new List<ParticleHit>();
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

            if (p.lifetime == p.startLifetime && environmentSetup.PieColors.Length > 0)
            {
                //Debug.Log(p.position);
                /*int index = (int)Random.Range(0, environmentSetup.PieColors.Length / 2);
                if (index >= environmentSetup.PieColors.Length / 2) index = environmentSetup.PieColors.Length / 2 - 1;
                p.color = Color.Lerp(environmentSetup.PieColors[index], environmentSetup.PieColors[index + environmentSetup.PieColors.Length / 2], Random.Range(0, 1.0F));*/
            }

            foreach (ParticleHit pos in hitParticlePos)
            {
                Vector3 dir = pos.Position - (p.position + ps.transform.position);
                if (dir.sqrMagnitude < 3000)
                {
                    //p.color = Color.grey;
                    //p.velocity = dir * 100;

                    int index = pos.ColorId;
                    p.color = Color.Lerp(environmentSetup.PieColors[index], environmentSetup.PieColors[index + environmentSetup.PieColors.Length / 2], Random.Range(0, 1.0F));
                }
            }

            return p;
        });

        hitParticlePos.Clear();


    }

    void OnParticleCollision(GameObject other)
    {
        //Debug.Log(LayerMask.LayerToName(other.layer));
        //Debug.Log(other);

        if (other.layer == LayerMask.NameToLayer("player"))
        {
            AvatarPlayer p = other.GetComponent<AvatarPlayer>();

            int safeLength = ps.GetSafeCollisionEventSize();
            if (collisionEvents.Length < safeLength)
                collisionEvents = new ParticleCollisionEvent[safeLength];

            int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);

            Vector3 pos = other.transform.position;
            pos.y = ps.transform.position.y;
            hitParticlePos.Add(new ParticleHit(pos, p.ColorId));

            /*int i = 0;
            while (i < numCollisionEvents)
            {
//                Vector3 pos = collisionEvents[i].intersection; //.colliderComponent.transform.position;
                hitParticlePos.Add(pos);

                //collisionEvents[i]
                //ps.SetParticles();
                i++;
            }*/
        }
    }
}
