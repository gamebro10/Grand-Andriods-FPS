using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("----- Player Stuff -----")]
    public GameObject player;
    public PlayerMovement playerMovement;
    public PlayerScript playerScript;
    public GameObject playerSpawnPos;

    [Header("----- UI Stuff -----")]
    public GameObject activeMenu;
    public GameObject prevMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject titleMenu;
    public GameObject optionsMenu;
    public GameObject levelSelect;
    public GameObject enemyHeader;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI speedometerText;
    public Image speedometerBar;
    public Image playerHPBar;
    public GameObject playerFlashDamageScreen;
    public BossHealthBar bossHealthBar;

    [Header("-----Options Stuff-----")]
    [SerializeField] public Slider FOVSlider;
    [SerializeField] public TextMeshProUGUI FOVText;
    public int FOVValue;
    [SerializeField] public Slider MouseSensSlider;
    [SerializeField] public TextMeshProUGUI MouseSensText;
    public int MouseSensValue;
    [SerializeField] public Slider SFXSlider;
    [SerializeField] public TextMeshProUGUI SFXText;
    public int SFXValue;
    [SerializeField] public Slider MusicSlider;
    [SerializeField] public TextMeshProUGUI MusicText;
    public int MusicValue;

    public int CameraFOV;
    int enemiesRemaining;
    public bool isPaused;
    float timescaleOrig;

    void Awake()
    {
        setOptionsSliders();
        setOptionsDefault();
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerScript = player.GetComponent<PlayerScript>();
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

    public void loadOptionsMenu()
    {
        prevMenu = activeMenu;
        activeMenu.SetActive(false);
        activeMenu = optionsMenu;
        activeMenu.SetActive(true);
    }

    public void setOptionsDefault()
    {
        FOVSlider.value = FOVValue = 60;
        //playerScript.playerCam.fieldOfView = FOVValue;
        FOVText.text = FOVValue.ToString();
        MouseSensSlider.value = MouseSensValue = 50;
        MouseSensText.text = MouseSensValue.ToString(); 
        SFXSlider.value = SFXValue = 100;
        SFXText.text = SFXValue.ToString();
        MusicSlider.value = MusicValue = 100;
        MusicText.text = MusicValue.ToString();
    }

    public void setOptionsSliders()
    {
        FOVSlider.onValueChanged.AddListener((v) => { FOVValue = (int)v; });
        //playerScript.playerCam.fieldOfView = FOVValue;
        MouseSensSlider.onValueChanged.AddListener((v) => { MouseSensValue = (int)v; });
        SFXSlider.onValueChanged.AddListener((v) => { SFXValue = (int)v; });
        MusicSlider.onValueChanged.AddListener((v) => {MusicValue = (int)v; });
    }

    public void closeOptions()
    {
        activeMenu.SetActive(false);
        activeMenu = prevMenu;
        activeMenu.SetActive(true);
        prevMenu = null;
    }

    public void updateEnemy(int amount)
    {
        enemiesRemaining += amount;
        enemiesRemainingText.text = enemiesRemaining.ToString();
        if (enemiesRemaining <= 0) 
        {
            statePaused();
            activeMenu = winMenu;
            activeMenu.SetActive(true);
        }

    }

    public IEnumerator playerFlashDamage()
    {
        playerFlashDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerFlashDamageScreen.SetActive(false);
    }
}
