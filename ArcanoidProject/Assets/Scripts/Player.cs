using UnityEngine;

public class Player : MonoBehaviour
{
    //Shows how fast is the platform moving
    public float xDelta;

    public GameObject Borders;

    //A place for reference to Collider of Borders and Platform
    private Collider2D BordersCollider, PlayerCollider;
    private string left = "left";
    private string right = "right";


    void Start()
    {
        //Check that the GameObject of Borders exists in the Inspector and fetch the Collider
        if (Borders != null)
            BordersCollider = Borders.GetComponent<Collider2D>();

        PlayerCollider = this.GetComponent<Collider2D>();
    }

    
    void Update()
    {
        //If the 'left' button pressed and there is no wall on platform's way
        if (Input.GetKey(left) && CheckCollide() != left)
        {
            Vector3 oldPosition = transform.position;
            transform.position = new Vector2(oldPosition.x - xDelta, oldPosition.y);
        }

        //If the 'right' button pressed and there is no wall on platform's way
        if (Input.GetKey(right) && CheckCollide() != right)
        {
            Vector3 oldPosition = transform.position;
            transform.position = new Vector2(oldPosition.x + xDelta, oldPosition.y);
        }
       
    }

    //For checking the collisions the coordinates of bounds of BoxCollider are used
    private string CheckCollide()
    {
        float leftSideOfPlayer = PlayerCollider.bounds.min.x;
        float rightSideOfPlayer = PlayerCollider.bounds.max.x;
        float leftBorder = BordersCollider.bounds.min.x;
        float rightBorder = BordersCollider.bounds.max.x;

        if (leftSideOfPlayer <= leftBorder)
        {
            return left;
        } 
        
        if (rightSideOfPlayer >= rightBorder)
        {
            return right;
        }

        return string.Empty;
    }
}
