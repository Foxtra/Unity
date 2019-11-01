using UnityEngine;

public class ExtraBall : MonoBehaviour
{
    private Vector3 velocity = new Vector3(0, -0.02f, 0);

    private GameObject Ball;

    private GameObject NewBall;
    
    private GameObject Player;

    private Collider2D BonuceCollider;

    private float timeSinceBonuceActivated;

    private bool bonuceActive = false;

    void Start()
    {
        BonuceCollider = this.GetComponent<Collider2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Ball = GameObject.FindGameObjectWithTag("Ball");
    }

    void Update()
    {
        //Checks if there is a collision with Platform
        CheckCollision();

        transform.position += velocity;

        //If bonus is active, turn it off after some time and make self-destroying
        if (bonuceActive)
        {
            timeSinceBonuceActivated += Time.deltaTime;
            if (timeSinceBonuceActivated >= GameController.instance.bonuceTime)
            {
                Destroy(NewBall.gameObject);
                Destroy(gameObject);
            }
        }
    }

    //For checking the collisions the coordinates of bounds of BoxCollider are used
    private void CheckCollision()
    {
        Collider2D PlayerCollider = Player.GetComponent<Collider2D>();

        if (PlayerCollider != null)
        {
            if (!bonuceActive
                && BonuceCollider.bounds.min.y <= PlayerCollider.bounds.max.y
                && BonuceCollider.bounds.max.x >= PlayerCollider.bounds.min.x
                && BonuceCollider.bounds.min.x <= PlayerCollider.bounds.max.x
                && BonuceCollider.bounds.max.y >= PlayerCollider.bounds.min.y)
            {
                NewBall = Instantiate(Ball, Ball.transform.position, Quaternion.identity);
                NewBall.GetComponent<Ball>().SetInitialVelocity();
                bonuceActive = true;
            }

            //If there is no collision with platform and the platform is above
            //make self-destroying
            if (!bonuceActive
               && BonuceCollider.bounds.max.y <= PlayerCollider.bounds.min.y)
            {
                Destroy(gameObject);
            }
        }
    }
}
