using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;

    // Cache
    private float baseTimeScale;

        // Audio
    private AudioSource music;
    private Text musicButtonText;

    private void Start()
    {
        baseTimeScale = Time.timeScale;
        music = GetComponent<AudioSource>();
        musicButtonText = pauseMenu.transform.Find("MusicButton").gameObject.GetComponentInChildren<Text>();
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

    public void ToggleMusic()
    {
        bool musicStatus = music.isPlaying;
        if (musicStatus)
            music.Pause();
        else
            music.Play();

        string suffix = music.isPlaying ? "on" : "off";
        musicButtonText.text = "music: " + suffix;
    }
}
