using UnityEngine;
using System.Collections;

public class MyEnemyDeath : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 1f;
    
    void Start()
    {
        Destroy(gameObject, _lifeTime);
    }
}
