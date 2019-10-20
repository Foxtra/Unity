using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    //A reference to our game controller script so we can access it statically.
    public static GameController instance;

    //A reference to the object that displays the text which appears when the player dies.
    public GameObject gameOverText;

    //A reference to the UI text component that displays the player's score.
    public Text scoreText;

    //Is the game over?
    public bool gameOver = false;

    //Speed of background
    public float scrollSpeed = -1.5f;

    private int score;
    
    void Awake()
    {
        //If we don't currently have a game control...
        if (instance == null)
        {
            //...set this one to be it...
            instance = this;
        }
        //...otherwise...
        else if ( instance != this)
        {
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        //If the game is over and the player has pressed some input...
        if (gameOver && Input.GetMouseButtonDown(0))
        {
            //...reload the current scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void BirdScored()
    {
        //The bird can't score if the game is over.
        if (gameOver)
        {
            return;
        }

        //If the game is not over, increase the score...
        score++;

        //...and adjust the score text.
        scoreText.text = "Score: " + score.ToString();
    }

    public void BirdDied()
    {
        //Activate the game over text.
        gameOverText.SetActive(true);

        //Set the game to be over.
        gameOver = true;
    }
}
