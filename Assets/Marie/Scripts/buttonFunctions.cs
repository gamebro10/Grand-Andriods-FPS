using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonfunctions : MonoBehaviour
{
    public void resumeGame()
    {
        GameManager.Instance.stateUnpaused();
    }

    public void respawn()
    {
        GameManager.Instance.stateUnpaused();
        PlayerScript.Instance.spawnPlayer();
    }

    public void restartGame()
    {
        GameManager.Instance.loadOptions();
        GameManager.Instance.stateUnpaused();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void mainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void options()
    {
        GameManager.Instance.loadOptionsMenu();
    }

    public void controls()
    {
        GameManager.Instance.loadControlsMenu();
    }

    public void closeOptions()
    {
        GameManager.Instance.closeOptions();
    }

    public void closeControls()
    {
        GameManager.Instance.closeControls();
    }

    public void levelSelect(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    public void nextLevel()
    {
        GameManager.Instance.stateUnpaused();
        levelSelect(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OptionsSave()
    {
        //GameManager.Instance.saveSetOptions();
    }

    public void OptionsDefault()
    {
        GameManager.Instance.setOptionsDefault();
    }
}
