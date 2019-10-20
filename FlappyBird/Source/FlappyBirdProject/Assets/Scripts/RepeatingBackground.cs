using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    //This stores a reference to the collider attached to the Ground.
    private BoxCollider2D groundCollider;

    //A float to store the x-axis length of the collider2D attached to the Ground GameObject.
    private float groundHorizontalLenght;
    
    void Start()
    {
        //Get and store a reference to the collider2D attached to Ground.
        groundCollider = GetComponent<BoxCollider2D>();

        //Store the size of the collider along the x axis (its length in units).
        groundHorizontalLenght = groundCollider.size.x;
    }

    
    void Update()
    {
        //Check if the difference along the x axis between the main Camera and 
        //the position of the object this is attached to is greater than groundHorizontalLength.
        if (transform.position.x < -groundHorizontalLenght)
        {
            // If true, this means this object is no longer visible and we can safely move it forward to be reused.
             RepositionBackground();
        }
    }

    //Moves the object this script is attached to right in order to create our looping background effect.
    private void RepositionBackground()
    {
        //This is how far to the right we will move our background object, in this case, twice its length. 
        //This will position it directly to the right of the currently visible background object.
        Vector2 groundOffset = new Vector2(groundHorizontalLenght * 2f, 0);

        //Move this object from it's position off-screen, behind the player, to the new position off-camera in front of the player.
        transform.position = (Vector2)transform.position + groundOffset;
    }
}
