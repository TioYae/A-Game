using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene(0);
    }

    public void loadGame()
    {
        
    }

    public void exitGame()
    {
        Application.Quit();
    }
}
