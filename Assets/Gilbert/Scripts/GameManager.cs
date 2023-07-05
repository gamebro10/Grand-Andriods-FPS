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
    public TextMeshProUGUI speedometerText;
    public Image speedometerBar;
    public Image playerHPBar;
    public GameObject playerFlashDamageScreen;

    int enemiesRemaining;
    bool isPaused;
    float timescaleOrig;

    void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        timescaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
    }


    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            statePaused();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }
    }

    public void statePaused()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = !isPaused;
    }

    public void stateUnpaused()
    {
        Time.timeScale = timescaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void youLose()
    {
        statePaused();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }

    public void updateEnemyl(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString();
    }

    public void updateSpeedometer(int amount)
    {
        //Rigidbody temp = playerScript.GetRb();
        //speedometerText = (temp.velocity.magnitude).ToString("0") + ("m/s");
    }
}
