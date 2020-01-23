using UnityEngine;
using System.Collections;

public class MyKey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            OpenDoor();
            Die();
        }
    }

    private void OpenDoor()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in doors)
        {
            if (door.name.Equals("Door"))
            {
                var foundedDoor = door.GetComponent<MyDoor>();
                foundedDoor.Die();
                break;
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
