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
    public GameObject cheatMenu;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI speedometerText;
    public Image speedometerBar;
    public Image playerHPBar;
    public GameObject playerFlashDamageScreen;
    public BossHealthBar bossHealthBar;

    [Header("-----Options Stuff-----")]
    [SerializeField] public SettingsStuff optionsvalues;
    [SerializeField] public SettingsStuff optionsspare;
    [SerializeField] public Slider FOVSlider;
    [SerializeField] public TextMeshProUGUI FOVText;
    [SerializeField] public Slider MouseSensSlider;
    [SerializeField] public TextMeshProUGUI MouseSensText;
    [SerializeField] public Slider SFXSlider;
    [SerializeField] public TextMeshProUGUI SFXText;
    [SerializeField] public Slider MusicSlider;
    [SerializeField] public TextMeshProUGUI MusicText;


    //public int CameraFOV;
    int enemiesRemaining;
    int batteriesRemaining;
    public bool isPaused;
    float timescaleOrig;

    void Awake()
    {
        loadOptions();
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerScript = player.GetComponent<PlayerScript>();
        }
        timescaleOrig = Time.timeScale;
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
#if true
        Instantiate(cheatMenu, transform);
#endif
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
        loadOptions();
        prevMenu = activeMenu;
        activeMenu.SetActive(false);
        activeMenu = optionsMenu;
        activeMenu.SetActive(true);
    }

    public void setOptionsDefault()
    {
        FOVSlider.value = optionsvalues.FOVValue = 60;
        //playerScript.playerCam.fieldOfView = FOVValue;
        FOVText.text = optionsvalues.FOVValue.ToString();
        MouseSensSlider.value = optionsvalues.MouseSensValue = 50;
        MouseSensText.text = optionsvalues.MouseSensValue.ToString(); 
        SFXSlider.value = optionsvalues.SFXValue = 100;
        SFXText.text = optionsvalues.SFXValue.ToString();
        MusicSlider.value = optionsvalues.MusicValue = 100;
        MusicText.text = optionsvalues.MusicValue.ToString();
    }

    public void setOptionsSliders()
    {
        FOVSlider.onValueChanged.AddListener((v) => { optionsvalues.FOVValue = (int)v; });
        //playerScript.playerCam.fieldOfView = FOVValue;
        MouseSensSlider.onValueChanged.AddListener((v) => { optionsvalues.MouseSensValue = (int)v; });
        SFXSlider.onValueChanged.AddListener((v) => { optionsvalues.SFXValue = (int)v; });
        MusicSlider.onValueChanged.AddListener((v) => { optionsvalues.MusicValue = (int)v; });
    }

    public void closeOptions()
    {
       saveOptions();
        activeMenu.SetActive(false);
        activeMenu = prevMenu;
        activeMenu.SetActive(true);
        prevMenu = null;
        loadOptions();
    }

    public void saveOptions()
    {
        optionsspare.FOVValue = optionsvalues.FOVValue;
        optionsspare.MouseSensValue = optionsvalues.MouseSensValue;
        optionsspare.SFXValue = optionsvalues.SFXValue;
        optionsspare.MusicValue = optionsvalues.MusicValue;
    }

    public void loadOptions()
    {
        setOptionsSliders();
        optionsvalues.FOVValue = optionsspare.FOVValue;
        optionsvalues.MouseSensValue = optionsspare.MouseSensValue;
        optionsvalues.SFXValue = optionsspare.SFXValue;
        optionsvalues.MusicValue = optionsspare.MusicValue;
       
     }

    public void updateEnemy(int amount)
    {
        batteriesRemaining += amount;
        enemiesRemainingText.text = batteriesRemaining.ToString();
        //if (enemiesRemaining <= 0) 
        //{
        //    statePaused();
        //    activeMenu = winMenu;
        //    activeMenu.SetActive(true);
        //}

    }

    public IEnumerator playerFlashDamage()
    {
        playerFlashDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerFlashDamageScreen.SetActive(false);
    }
}
