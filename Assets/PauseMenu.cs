using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject InfomenuUI;

    bool menuOpen = false;
    bool infoOpen = false;
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (gameIsPaused && !infoOpen)
            {
                ResumeInf();
                menuOpen = false;
            }
            else if(!infoOpen)
            {
                PauseInf();
                menuOpen = true;
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused && !menuOpen)
            {
                Resume();
                infoOpen = false;
            }
            else if(!menuOpen)
            {
                Pause();
                infoOpen = true;
            }
        }
	}

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void ResumeInf()
    {
        InfomenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void PauseInf()
    {
        InfomenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

}
