using UnityEngine;
using System.Collections;

public class MyCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_player.position.x, _player.position.y, transform.position.z);
    }
}
