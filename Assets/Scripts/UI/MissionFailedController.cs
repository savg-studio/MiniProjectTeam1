using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionFailedController : MonoBehaviour
{
    public float exitTimeOffset;
    public float audioPlayTimeOffset;

    // Components
    private TextFader textFader;
    private AudioSource audioClip;

    // Start is called before the first frame update
    void Start()
    {
        textFader = GetComponentInChildren<TextFader>();
        audioClip = GetComponent<AudioSource>();

        textFader.SetOnFadeIn(QuitGame);
        textFader.Fade();

        Invoke("PlayAudio", audioPlayTimeOffset);
    }

    private void QuitGame()
    {
        Invoke("DoQuitGame", exitTimeOffset);
    }

    private void DoQuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void PlayAudio()
    {
        audioClip.Play();
    }
}
