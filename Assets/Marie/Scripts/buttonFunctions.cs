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
        GameManager.Instance.playerScript.spawnPlayer();
    }

    public void restartGane()
    {
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
        switch (levelNumber)
        {
            default: // default just in case
                break;
            case 0: //tutorial
                break;
            case 1: //level 1
                break;
            case 2: //level 2
                break;
            case 3: //level 3
                break;
            case 4: // level 4
                break;
        }
    }

    public void nextLevel()
    {
        int currentLevel = 0; // just 0 for now so VS doesnt get angry at me
        //find a way to get current level from either game manager or somewhere else in the scene.
        levelSelect(currentLevel + 1);
    }
}
