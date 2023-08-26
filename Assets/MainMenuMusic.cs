using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMusic : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioClip mainMenuClip;
    public AudioClip levelClip;
    public AudioClip bossClip;


    public static MainMenuMusic Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            AudioManager.Instance.RegisterMusic(backgroundMusic);
            SceneManager.sceneLoaded += delegate { OnSceneChanged(); };
            DontDestroyOnLoad(gameObject);
        }   
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneChanged()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (backgroundMusic.clip != mainMenuClip)
            {
                backgroundMusic.clip = mainMenuClip;
                backgroundMusic.Play();
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            if (backgroundMusic.clip != bossClip)
            {
                backgroundMusic.clip = bossClip;
                backgroundMusic.Play();
            }
        }
        else
        {
            if (backgroundMusic.clip != levelClip)
            {
                backgroundMusic.clip = levelClip;
                backgroundMusic.Play();
            }
        }
    }
}
