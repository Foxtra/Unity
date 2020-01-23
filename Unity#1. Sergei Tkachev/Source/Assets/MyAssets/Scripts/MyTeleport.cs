using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MyTeleport : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
