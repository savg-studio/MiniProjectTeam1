using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;

    private float baseTimeScale;

    private void Start()
    {
        baseTimeScale = Time.timeScale;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {// Escape
            if (pauseMenu.activeInHierarchy)
                Resume();
            else
                Pause();

        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = baseTimeScale;
    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
