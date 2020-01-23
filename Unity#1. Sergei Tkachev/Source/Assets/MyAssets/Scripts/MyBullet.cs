using UnityEngine;
using System.Collections;

public class MyBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _lifeTime = 4;
    [SerializeField] private int _damage = 1;
    private Character _player;
    private bool _isRightDirection;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, _lifeTime);
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        _isRightDirection = _player.IsRightDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRightDirection)
        {
            transform.position += Vector3.right * _speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * _speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<MyEnemy>();
            enemy.Hurt(_damage);
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
