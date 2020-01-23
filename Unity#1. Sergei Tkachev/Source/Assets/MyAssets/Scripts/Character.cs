using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _startBullet;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _jumpVelocity = 35;
    [SerializeField] private LayerMask platformLayerMask;
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private Vector3 _respawn = new Vector3(-7, -1, 0);
    private float _deadline = -20;
    public bool IsRightDirection { get; set; } = true;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.position += Vector3.right * horizontalInput * _speed * Time.deltaTime;

        if(horizontalInput < 0 && IsRightDirection)
        {
            Flip();
        }
        else if (horizontalInput > 0 && !IsRightDirection)
        {
            Flip();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(_bullet, _startBullet.position, _startBullet.rotation);
        }

        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.up * _jumpVelocity;
        }

        CheckPlayersFall();
    }

    private bool IsGrounded()
    {
        RaycastHit2D[] raycastHits2d = Physics2D.CapsuleCastAll(cc.bounds.center, cc.bounds.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, .1f, platformLayerMask);
        foreach(var cast in raycastHits2d)
        {
            if (cast.collider.name.Contains("Platform"))
            {
                return true;
            }
        }

        return false;
    }

    private void CheckPlayersFall()
    {
        if(transform.position.y < _deadline)
        {
            transform.position = _respawn;
        }
    }

    private void Flip()
    {
        IsRightDirection = !IsRightDirection;
        transform.Rotate(0f, 180f, 0f);
    }
}
