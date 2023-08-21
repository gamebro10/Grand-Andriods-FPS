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
}
