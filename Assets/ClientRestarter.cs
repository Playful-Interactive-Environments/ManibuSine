using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClientRestarter : MonoBehaviour {

	void Start () {
        Debug.LogError("Restarter");

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        StartCoroutine(RestartDelayed());
    }

    IEnumerator RestartDelayed() {
        yield return new WaitForSeconds(1.9f);
        Debug.LogError("Load Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
