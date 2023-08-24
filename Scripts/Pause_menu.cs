using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_menu : MonoBehaviour
{
    private bool GameIsPaused = false;
    public GameObject Pause_menuUI;

    public void Button()
    {
        if (GameIsPaused)
        {
            Resume();
        }

        else
        {
            Pause();
        }
    }

    private void Resume()
    {
        Pause_menuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    
    private void Pause()
    {
        Pause_menuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
