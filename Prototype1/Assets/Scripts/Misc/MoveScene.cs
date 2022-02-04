using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public int sceneIndex;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Ship") {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
