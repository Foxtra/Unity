using UnityEngine;

public class Block : MonoBehaviour
{

    private int health;

    //Array of prefabs of game bonuses
    public GameObject[] prefabsOfBonuses;

    private bool containedBonuse = false;

    //Sets health of block
    public void SetHealth(int value)
    {
        this.health = value;
    }

    //Takes damage by ball, checks if there are some bonuses and dies
    public void GetDamage()
    {
        this.health--;
        if (health <= 0)
        {
            GenerateBonuce();
            Destroy(gameObject);
        }
    }

    //Create a bonus object
    public void GenerateBonuce()
    {
        int numOfPrefab = prefabsOfBonuses.Length;
        if (containedBonuse)
        {
            numOfPrefab = Random.Range(0, numOfPrefab);
            GameObject createdBonuce = Instantiate(prefabsOfBonuses[numOfPrefab], transform.position, Quaternion.identity);
        }
    }

    //Sets if this block has some bonus
    public void SetBonuce()
    {
        containedBonuse = true;
    }
    
}
