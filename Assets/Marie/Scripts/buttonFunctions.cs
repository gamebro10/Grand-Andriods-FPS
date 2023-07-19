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
        //load main menu scene
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void options()
    {
        GameManager.Instance.loadOptionsMenu();
    }

    public void closeOptions()
    {
        GameManager.Instance.closeOptions();
    }

    public void levelSelect(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    public void nextLevel()
    {
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
