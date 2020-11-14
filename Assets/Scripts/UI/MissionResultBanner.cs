using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionResultBanner : MonoBehaviour
{
    public float exitTimeOffset;
    public float audioPlayTimeOffset;

    public Color winTextColor;
    public Color loseTextColor;

    // Components
    private TextFader textFader;
    private AudioSource audioClip;

    // Cache
    private Text textBanner;
    private GameObject background;

    // Start is called before the first frame update
    void Awake()
    {
        textFader = GetComponentInChildren<TextFader>(true);
        audioClip = GetComponent<AudioSource>();

        // Cache
        textBanner = GetComponentInChildren<Text>(true);
        background = transform.Find("Background").gameObject;
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

    public void OnWin()
    {
        textBanner.color = winTextColor;
        textBanner.text = "Mission done";

        textFader.SetOnFadeIn(QuitGame);
        textFader.Fade();

        textBanner.gameObject.SetActive(true);
        background.SetActive(true);

        QuitGame();
    }

    public void OnFail()
    {
        textBanner.color = loseTextColor;
        textBanner.text = "Mission failed";

        textFader.SetOnFadeIn(QuitGame);
        textFader.Fade();

        textBanner.gameObject.SetActive(true);
        background.SetActive(true);

        Invoke("PlayAudio", audioPlayTimeOffset);
        QuitGame();
    }
}
