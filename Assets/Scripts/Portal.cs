using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Medieval":
                SceneManager.LoadScene("WW2");
                break;
            case "WW2":
                SceneManager.LoadScene("SpaceStation");
                break;
            default:
                break;
        }
    }
}
