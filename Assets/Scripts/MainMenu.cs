using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel(string whatLevel)
    {
        SceneManager.LoadScene(whatLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    
}
