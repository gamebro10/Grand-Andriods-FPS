using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonfunctions : MonoBehaviour
{
    public void resumeGame()
    {

    }

    public void respawn()
    {

    }

    public void restartGane()
    {

    }

    public void mainMenu()
    {

    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void options()
    {

    }

    public void levelSelect(int levelNumber)
    {
        switch (levelNumber)
        {
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
        //find a way to get current level from either game manager or somewhere else in the scene,
        //and set currentLeve equal to that plus one.
        levelSelect(currentLevel);
    }
}
