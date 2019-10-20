using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class musicPlayer : MonoBehaviour {

    public Slider Volume;
    public AudioSource gameMusic;
    public AudioSource winMusic;
    private float volume;

    private void Awake()
    {
            DontDestroyOnLoad(transform.gameObject);
    }

    void OnLevelWasLoaded()
    {
        if((gameObject.name == "musicGame" && SceneManager.GetActiveScene().buildIndex == 2) || (gameObject.name == "musicWin" && SceneManager.GetActiveScene().buildIndex == 0))
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if(Volume != null)
        {
            volume = Volume.value;
        }

        if (gameObject.name == "musicGame")
        {
            gameMusic.volume = volume;
        }
        else
        {
            winMusic.volume = volume;
        }
    }

}
