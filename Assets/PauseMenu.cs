using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject InfomenuUI;
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (gameIsPaused)
            {
                ResumeInf();
            }
            else
            {
                PauseInf();
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else {
                Pause();
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
