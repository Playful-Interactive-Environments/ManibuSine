using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Canon : MonoBehaviour {

    public GameObject bulletPrefab;

    void Start()
    {
        NetworkDataManager.Instance.EventShoot += Shoot;
    }
    
    void Shoot()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + transform.up, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().velocity = transform.up * 1000.0f;
        Destroy(bullet, 10.0f);

        //NetworkServer.Spawn(bullet);
    }
}