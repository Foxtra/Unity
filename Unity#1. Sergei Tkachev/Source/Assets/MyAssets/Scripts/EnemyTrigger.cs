using UnityEngine;
using System.Collections;

public class EnemyTrigger : MonoBehaviour
{
    public GameObject enemy;
    [SerializeField] private Sprite _active;
    [SerializeField] private Sprite _inactive;
    [SerializeField] private Vector3 _enemyPosition = new Vector3 ( 3, -3, 0 );
    private bool isActive = false;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (!isActive)
            {
                spriteRenderer.sprite = _active;
                isActive = !isActive;
                Instantiate(enemy, _enemyPosition, transform.rotation);
            }
        }
    }

    public void SetInactive()
    {
        isActive = false;
        spriteRenderer.sprite = _inactive;
    }
}
