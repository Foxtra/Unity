using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //A reference to our game controller script so we can access it statically.
    public static GameController instance;

    //A reference to the object that displays the text which appears when the player loses.
    public GameObject gameOverText;
    
    //A reference to the object that displays the text which appears when the player wins.
    public GameObject gameWinText;

    //A reference to the object that displays the introductions text.
    public GameObject welcomeText;

    //A reference to the UI button component that displays when the player loses.
    public Button restartButton;

    //A reference to the prefab object for block.
    public GameObject blockPrefab;

    //A reference to the UI text component that displays the player's score.
    public Text levelText;

    //Bonus duration time
    public float bonuceTime = 10f;

    //Shows if the game process was started or not
    public bool gameStarted = false;

    //Shows if the game was over
    public bool gameOver = false;

    public int amountOfBlocks = 10;

    //Min health of one block
    public int minHealth = 1;

    //Shows the current level. Static for saving value when Scene is reloading
    private static int level = 1;

    //Max level in this game
    private int maxLevel = 3;

    //Shows if level was completed (all blocks were destroyed)
    private bool levelCompleted = false;

    private GameObject[] blocks;


    void Awake()
    {
        //If we don't currently have a game control...
        if (instance == null)
        {
            //...set this one to be it...
            instance = this;
        }
        //...otherwise...
        else if (instance != this)
        {
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //After game starts, Block are generated
        GenerateBlocks();
    }

    void Update()
    {
        //If the level was completed...
        if (CheckWin())
        {
            //... move to the next level via restarting scene
            RestatGame();
        }

        //If the game has already started..
        if (gameStarted)
        {
            //.. remove introduction text
            welcomeText.SetActive(false);
        }

        //Display current level
        levelText.text = "Level: " + level.ToString();
    }

    public void RestatGame()
    {
        //If the game is over..
        if (gameOver)
        {
            //.. reload the current scene and reset levet to 1.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            level = 1;
        }

        //If the level is completed, increase the level
        if (levelCompleted)
        {
            level++;

            //Stop the game if the last level has been completed
            if(level > maxLevel)
            {
                gameWinText.SetActive(true);
                GameStop();
            }
            else
            {
                //..reload the current scene.
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void BallLost()
    {
        //Activate the game over text.
        gameOverText.SetActive(true);

        //Activate the game over button.
        restartButton.gameObject.SetActive(true);

        //Tells the game that it's over
        gameOver = true;
    }

    //Stops movement of all balls
    private void GameStop()
    {
        GameObject[] activeBalls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (var ball in activeBalls)
        {
            ball.GetComponent<Ball>().Stop();
        }
    }

    //Checks if there are any blocks exists
    private bool CheckWin()
    {
        GameObject checkBlocks = GameObject.FindGameObjectWithTag("Block");

        if (checkBlocks == null)
        {
            levelCompleted = true;
        }

        return levelCompleted;
    }

    //Creates the blocks on the scene
    private void GenerateBlocks()
    {
        Collider2D bordersCollider = GameObject.FindGameObjectWithTag("Borders").GetComponent<Collider2D>();
        Renderer rendererCollider = blockPrefab.GetComponent<Renderer>();
        int numbersOfRows = Mathf.FloorToInt(bordersCollider.bounds.size.x / (rendererCollider.bounds.size.x * 1.4f));
        int numbersOfStrings = Mathf.CeilToInt((float)amountOfBlocks / (float)numbersOfRows);

        blocks = new GameObject[amountOfBlocks]; 
        for (int i = 0; i < numbersOfStrings; i++)
        {
            if(numbersOfStrings - 1 == i)
            {
                numbersOfRows = amountOfBlocks - (numbersOfRows * (numbersOfStrings - 1));
            }

            for (int j = 0; j < numbersOfRows; j++)
            {
                if (numbersOfStrings - 1 == i)
                {
                    blocks[i * j + j] = Instantiate(blockPrefab,
                         new Vector3(bordersCollider.bounds.center.x + 
                                        rendererCollider.bounds.extents.x * (j + 1) * ((((j)%2)*2)-1) +
                                        rendererCollider.bounds.extents.x * ((j+1) % 2),
                                     bordersCollider.bounds.max.y - rendererCollider.bounds.size.y * 1.5f * (i + 1),
                                     0),
                                     Quaternion.identity);
                    blocks[i * j + j].gameObject.GetComponent<Block>().SetHealth(GenerateHealth(minHealth, level));
                    if(level == maxLevel)
                    {
                        if (Random.Range(0, 2) % 2 == 0)
                        {
                            blocks[i * j + j].gameObject.GetComponent<Block>().SetBonuce();
                        }
                    }
                }
                else
                {
                    blocks[i * j + j] = Instantiate(blockPrefab,
                        new Vector3(bordersCollider.bounds.min.x + rendererCollider.bounds.size.x * 1.4f * (j + 1),
                                    bordersCollider.bounds.max.y - rendererCollider.bounds.size.y * 1.5f * (i + 1),
                                    0),
                                    Quaternion.identity);
                    blocks[i * j + j].gameObject.GetComponent<Block>().SetHealth(GenerateHealth(minHealth, level));
                    if (level == maxLevel)
                    {
                        if (Random.Range(0, 2) % 2 == 0)
                        {
                            blocks[i * j + j].gameObject.GetComponent<Block>().SetBonuce();
                        }
                    }
                }
            }
            
        }
    }

    //Generates health value between min and max (including)
    private int GenerateHealth(int minHealth, int maxHealth)
    {
        int health = Random.Range(minHealth, maxHealth+1);

        return health;
    }
}
