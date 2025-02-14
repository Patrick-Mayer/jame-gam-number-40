using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    [SerializeField] private GameObject UI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                Resume();
            }else{
                Pause();
            }
        }
    }

    public void Resume(){
        UI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause() {
        UI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitGame(){
        Application.Quit();
    }
}
