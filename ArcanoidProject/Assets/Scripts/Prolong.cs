using UnityEngine;

public class Prolong : MonoBehaviour
{
    public Sprite newSprite;

    public Sprite oldSprite;

    private Vector3 velocity = new Vector3(0, -0.02f, 0);

    private GameObject Player;

    private Collider2D BonuceCollider;

    private float timeSinceBonuceActivated;

    private bool bonuceActive = false;

    void Start()
    {
        BonuceCollider = this.GetComponent<Collider2D>();
        Player = GameObject.FindGameObjectWithTag("Player");
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
                Player.GetComponent<SpriteRenderer>().sprite = oldSprite;
                Player.GetComponent<BoxCollider2D>().size = new Vector2(
                    Player.GetComponent<SpriteRenderer>().bounds.size.x,
                    Player.GetComponent<SpriteRenderer>().bounds.size.y
                    );
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
                Player.GetComponent<SpriteRenderer>().sprite = newSprite;
                Player.GetComponent<BoxCollider2D>().size = new Vector2(
                    Player.GetComponent<SpriteRenderer>().bounds.size.x,
                    Player.GetComponent<SpriteRenderer>().bounds.size.y
                    );

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
