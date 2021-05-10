using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public GameObject deathMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !deathMenuUI.activeSelf)
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        } 
    }

    public void ShowDeathScreen()
    {
        deathMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = false;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        Destroy(SceneLoader.instance.gameObject);
    }

    public void QuitGame()
    {
        Debug.Log("Exit!");
        Application.Quit();
    }

    public void RestartGame()
    {
        deathMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SceneLoader.instance.ChangeScene("Main");
    }
}
