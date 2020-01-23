using UnityEngine;
using System.Collections;

public class MyEnemy : MonoBehaviour
{
    [SerializeField] private int _health = 2;
    [SerializeField] private GameObject _deathEffect;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Hurt(int damage)
    {
        _health -= damage;

        StartCoroutine(changeColor());

        if(_health <= 0)
        {
            Die();
        }
    }

    IEnumerator changeColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSecondsRealtime(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        GameObject[] triggers = GameObject.FindGameObjectsWithTag("Trigger");
        foreach (GameObject trigger in triggers)
        {
            if (trigger.name.Equals("EnemyTrigger"))
            {
                var enemy = trigger.GetComponent<EnemyTrigger>();
                enemy.SetInactive();
                break;
            }
        }

        Instantiate(_deathEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
