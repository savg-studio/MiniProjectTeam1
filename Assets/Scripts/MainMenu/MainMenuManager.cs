﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum IntroState
{
    INITIAL_WAIT,
    HEADER_FADE_IN,
    MENU_FADE_IN,
    DONE
}

public class MainMenuManager : MonoBehaviour
{
    public Text header;
    public Text footer;
    public GameObject menu;
    public GameObject newGameMenu;

    public float waitFadeDuration;
    public float headerFadeDuration;
    public float menuFadeDuration;

    // Backup
    private Color baseHeaderColor;
    private Color baseButtonColor;

    // State
    private IntroState state;

    // Cache
    private List<Button> menuButtons;
    private float timePassed;

    private void Start()
    {
        // Init flags
        if (headerFadeDuration != 0f && menuFadeDuration != 0f)
        {
            state = IntroState.INITIAL_WAIT;

            // Cache
            menuButtons = menu.GetComponentsInChildren<Button>().ToList();
            timePassed = 0;

            // Backup
            baseHeaderColor = header.color;
            baseButtonColor = menuButtons[0].GetComponentInChildren<Text>().color;

            // Init
            SetTextAlpha(header, 0);
            SetTextAlpha(footer, 0);
            SetButtonsAlpha(0);
            SetButtonsEnabled(false);
        }
        else
            state = IntroState.DONE;
    }

    private void Update()
    {
        timePassed += Time.deltaTime;

        switch(state)
        {
            case IntroState.INITIAL_WAIT:

                if (timePassed >= waitFadeDuration)
                {
                    state = IntroState.HEADER_FADE_IN;
                    timePassed = 0;
                }

                break;

            case IntroState.HEADER_FADE_IN:

                if (FadeInHeader())
                {
                    state = IntroState.MENU_FADE_IN;
                    timePassed = 0;
                }

                break;

            case IntroState.MENU_FADE_IN:

                
                if (FadeInButtonsAndFooter())
                {
                    state = IntroState.DONE;
                    SetButtonsEnabled(true);
                }

                break;

            case IntroState.DONE:
            default:

                break;
        }
    }

    private bool FadeInHeader()
    {
        float ratio = Mathf.Min(timePassed / headerFadeDuration, 1f);
        float currentAlpha = baseHeaderColor.a * ratio;

        SetTextAlpha(header, currentAlpha);
        

        return ratio >= 1f;
    }

    private void SetTextAlpha(Text text, float alpha)
    {
        Color currentColor = text.color;
        currentColor.a = alpha;
        text.color = currentColor;
    }

    private bool FadeInButtonsAndFooter()
    {
        float ratio = Mathf.Min(timePassed / menuFadeDuration, 1f);
        float currentAlpha = baseButtonColor.a * ratio;

        SetButtonsAlpha(currentAlpha);
        SetTextAlpha(footer, currentAlpha);

        return ratio >= 1f;
    }

    private void SetButtonsAlpha(float alpha)
    {
        foreach (var button in menuButtons)
        {
            SetTextAlpha(button.GetComponentInChildren<Text>(), alpha);
        }
    }

    private void SetButtonsEnabled(bool enabled)
    {
        foreach (var button in menuButtons)
        {
            // This is done to prevent weird stuff with colors
            button.image.enabled = enabled;
            button.enabled = enabled;
        }
    }

    public void OnNewGameClick()
    {
        ActivateNewGameMenu();
        menu.SetActive(false);
    }

    public void OnCloseGameClick()
    {
        Application.Quit();
    }

    public void OnRaceGameClick(AudioSource source)
    {
        source.Play();
        var length = source.clip.length;
        SetButtonsEnabled(false);
        Invoke("EnterMainGame", length);
    }

    public void OnScavengeGameClick(AudioSource source)
    {
        source.Play();
        var length = source.clip.length;
        SetButtonsEnabled(false);
        Invoke("EnterScavengeGame", length);
    }

    public void OnGoBackClick()
    {
        newGameMenu.SetActive(false);
        ActivateMenu();
    }

    public void ActivateNewGameMenu()
    {
        newGameMenu.SetActive(true);
    }

    public void ActivateMenu()
    {
        menu.SetActive(true);
    }

    public void EnterMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void EnterScavengeGame()
    {
        SceneManager.LoadScene("MainGame-CollectMode");
    }
}
