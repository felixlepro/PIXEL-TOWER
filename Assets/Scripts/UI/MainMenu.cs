﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider loadSlider;
    public Text loadText;

    public void PlayGame()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadAsynchronously(1));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadAsynchronously(0));
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadAsynchronously(1));
    }

    IEnumerator LoadAsynchronously (int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadSlider.value = progress;
            loadText.text = Mathf.Floor(progress*100) + "% ";
            yield return null;
        }
        
    }
}
