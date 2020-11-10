using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnNewGameClick(AudioSource source)
    {
        source.Play();
        var length = source.clip.length;
        Invoke("EnterMainGame", length);
    }

    public void OnCloseGameClick()
    {
        Application.Quit();
    }

    public void EnterMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}
