using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("----- Player Stuff -----")]
    public GameObject player;
   public PlayerMovement playerScript;
    public GameObject playerSpawnPos;

    [Header("----- UI Stuff -----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject titleMenu;
    public GameObject optionsMenu;
    public GameObject levelSelect;
    public TextMeshProUGUI enemiesRemainingText;
    public Image playerHPBar;
    public GameObject playerFlashDamageScreen;

    int enemiesRemaining;
    bool isPaused;
    float timescaleOrig;
    int tempHP = 10;

    void Awake()
    {
        
    }


    void Update()
    {
        
    }
}
