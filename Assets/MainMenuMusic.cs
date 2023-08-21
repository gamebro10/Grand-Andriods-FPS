using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    public AudioSource backgroundMusic;

    void Start()
    {
        AudioManager.Instance.RegisterMusic(backgroundMusic);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.UnregisterMusic(backgroundMusic);
        }
    }



}

