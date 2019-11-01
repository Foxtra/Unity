using UnityEngine;

public class Ball : MonoBehaviour
{
    public float xVelocityMin = -0.1f;
    public float xVelocityMax = 0.1f;
    public float yVelocityMax = 0.11f;
    public float yVelocityMin = 0.1f;

    private Vector3 ballVelocity = Vector3.zero;
    private const string enter = "return";
    private const string left = "left";
    private const string right = "right";
    private const string top = "top";
    private const string bottom = "bottom";

    //A place for reference to Collider of Ball
    private Collider2D BallCollider;

    void Start()
    {
        BallCollider = this.GetComponent<Collider2D>();
    }
    
    void Update()
    {
        //If the game isn't over
        if (!GameController.instance.gameOver)
        {
            //..and it isn't started and player has pressed 'enter'
            if (!GameController.instance.gameStarted && Input.GetKey(enter))
            {
                //set random velocity to ball
                SetInitialVelocity();

                //and mark that the game was started
                GameController.instance.gameStarted = true;
            }

            //don't forget to move ball with platform till the game is started
            else if (!GameController.instance.gameStarted && !Input.GetKey(enter))
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                transform.position = player.transform.position;
            }

            //Correct velocity if the collision was happened
            CorrectVelocity();

            transform.position += ballVelocity;
        }
    }

    public void IncreaseVelocity()
    {
        this.ballVelocity *= 2f;
    }

    public void DecreaseVelocity()
    {
        this.ballVelocity /= 2f ;
    }

    public void SetInitialVelocity()
    {
        ballVelocity = new Vector3(Random.Range(xVelocityMin, xVelocityMax), Random.Range(yVelocityMin, yVelocityMax), 0);
    }

    public void Stop()
    {
        ballVelocity = Vector3.zero;
    }

    private void CorrectVelocity()
    {
        string collision = CheckCollision();
        switch (collision)
        {
            case left:
                ballVelocity.x *= -1;
                return;
            case right:
                ballVelocity.x *= -1;
                return;
            case top:
                ballVelocity.y *= -1;
                return;
            case bottom:
                ballVelocity.y *= -1;
                return;
            default:
                break;
        }
    }

    //For checking the collisions the coordinates of bounds of BoxCollider are used
    private string CheckCollision()
    {
        //Check if the ball collides with borders of game field
        GameObject Borders = GameObject.FindGameObjectWithTag("Borders");
        Collider2D BordersCollider = Borders.GetComponent<Collider2D>();
        if (BordersCollider != null)
        {
            if (BallCollider.bounds.min.x <= BordersCollider.bounds.min.x)
            {
                float correctedX = BordersCollider.bounds.min.x + BallCollider.bounds.extents.x ;
                transform.position = new Vector3(correctedX, transform.position.y, 0);
                return left;
            }

            if (BallCollider.bounds.max.x >= BordersCollider.bounds.max.x)
            {
                float correctedX = BordersCollider.bounds.max.x - BallCollider.bounds.extents.x ;
                transform.position = new Vector3(correctedX, transform.position.y, 0);
                return right;
            } 
                
            if (BallCollider.bounds.max.y >= BordersCollider.bounds.max.y)
            {
                float correctedY = BordersCollider.bounds.max.y - BallCollider.bounds.extents.y;
                transform.position = new Vector3(transform.position.x, correctedY, 0);
                return top;
            }

            if (BallCollider.bounds.min.y <= BordersCollider.bounds.min.y)
            {
                float correctedY = BordersCollider.bounds.min.y + (BallCollider.bounds.extents.y);
                transform.position = new Vector3(transform.position.x, correctedY, 0);
                GameController.instance.BallLost();
                Stop();
                return string.Empty;
            }
        }

        //Checks if the ball collides with the platform
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Collider2D PlayerCollider = Player.GetComponent<Collider2D>();

        if (PlayerCollider != null)
        {
            if (BallCollider.bounds.min.y <= PlayerCollider.bounds.max.y 
                && BallCollider.bounds.max.x >= PlayerCollider.bounds.min.x
                && BallCollider.bounds.min.x <= PlayerCollider.bounds.max.x)
            {
                float correctedY = PlayerCollider.bounds.max.y + BallCollider.bounds.extents.y ;
                transform.position = new Vector3(transform.position.x, correctedY, 0);
                return top;
            }
        }

        //checks if the Ball collides with any of blocks
        GameObject[] Blocks = GameObject.FindGameObjectsWithTag("Block");

        for (int i = 0; i < Blocks.Length; i++)
        {
            Collider2D blockCollider = Blocks[i].GetComponent<Collider2D>();

            if (BallCollider.bounds.min.y <= blockCollider.bounds.max.y
                && BallCollider.bounds.max.x >= blockCollider.bounds.min.x
                && BallCollider.bounds.min.x <= blockCollider.bounds.min.x
                && BallCollider.bounds.max.y >= blockCollider.bounds.min.y)
            {
                Blocks[i].gameObject.GetComponent<Block>().GetDamage();
                return left;
            }

            if (BallCollider.bounds.min.y <= blockCollider.bounds.max.y
                && BallCollider.bounds.min.x <= blockCollider.bounds.max.x
                && BallCollider.bounds.max.x >= blockCollider.bounds.max.x
                && BallCollider.bounds.max.y >= blockCollider.bounds.min.y)
            {
                Blocks[i].gameObject.GetComponent<Block>().GetDamage();
                return right;
            }

            if (BallCollider.bounds.min.y <= blockCollider.bounds.max.y
                && BallCollider.bounds.min.x <= blockCollider.bounds.max.x
                && BallCollider.bounds.max.x >= blockCollider.bounds.min.x
                && BallCollider.bounds.max.y >= blockCollider.bounds.max.y)
            {
                Blocks[i].gameObject.GetComponent<Block>().GetDamage();
                return top;
            }

            if (BallCollider.bounds.max.y >= blockCollider.bounds.min.y
                && BallCollider.bounds.min.x <= blockCollider.bounds.max.x
                && BallCollider.bounds.max.x >= blockCollider.bounds.min.x
                && BallCollider.bounds.min.y <= blockCollider.bounds.min.y)
            {
                Blocks[i].gameObject.GetComponent<Block>().GetDamage();
                return bottom;
            }
        }

        return string.Empty;
    }
}
