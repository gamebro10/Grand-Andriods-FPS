using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public AudioSource levelMusic;

    void Start()
    {
        AudioManager.Instance.RegisterMusic(levelMusic);
    }

}
