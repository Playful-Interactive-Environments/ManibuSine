using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClientRestarter : MonoBehaviour {

	void Start () {
        StartCoroutine(RestartDelayed());
	}

    IEnumerator RestartDelayed() {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
