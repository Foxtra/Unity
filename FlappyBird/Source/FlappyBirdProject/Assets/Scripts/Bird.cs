using UnityEngine;

public class Bird : MonoBehaviour
{
    public float upForce;           //Upward force of the "flap".

    public float flapRate;          //How quickly bird can "flap".

    private bool isDead = false;   //Has the player collided with a wall or ground?

    private Rigidbody2D player;    //Holds a reference to the Rigidbody2D component of the bird.

    private Animator anim;         //Reference to the Animator component.

    private float timeSinceLastFlapped;

    void Start()
    {
        //Get and store a reference to the Rigidbody2D attached to this GameObject.
        player = GetComponent<Rigidbody2D>();

        //Get reference to the Animator component attached to this GameObject.
        anim = GetComponent<Animator>();

        //Initial value for not to wait until first "flap" will be able.
        timeSinceLastFlapped = flapRate;
    }


    void Update()
    {
        timeSinceLastFlapped += Time.deltaTime;

        //Don't allow control if the bird has died.
        if (!isDead)
        {
            //Look for input to trigger a "flap".
            if (Input.GetMouseButton(0) && timeSinceLastFlapped >= flapRate)
            {

                timeSinceLastFlapped = 0f;

                //...tell the animator about it and then...
                anim.SetTrigger("Flap");

                //...zero out the birds current y velocity before...
                player.velocity = Vector2.zero;

                //..giving the bird some upward force.
                player.AddForce(new Vector2(0, upForce));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        player.velocity = Vector2.zero;

        // If the bird collides with something set it to dead...
        isDead = true;

        //...tell the Animator about it...
        anim.SetTrigger("Die");

        GameController.instance.BirdDied();
    }
}
