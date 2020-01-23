using UnityEngine;
using System.Collections;

public class MyEnemySmart : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private float stoppingDistance = 5;

    private Transform target;

    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
