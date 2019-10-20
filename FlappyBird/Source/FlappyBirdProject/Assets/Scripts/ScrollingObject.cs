using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D ground;

    void Start()
    {
        //Get and store a reference to the Rigidbody2D attached to this GameObject.
        ground = GetComponent<Rigidbody2D>();

        //Start the object moving.
        ground.velocity = new Vector2 (GameController.instance.scrollSpeed, 0);
    }

   
    void Update()
    {
        // If the game is over, stop scrolling.
        if (GameController.instance.gameOver)
        {
            ground.velocity = Vector2.zero;
        }
    }
}
