using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        if (SceneManager.GetActiveScene().name == "WinScreen")
        {
            SceneManager.LoadScene("DynamicFollow");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void QuitGame(){
        Application.Quit();
    }
}
