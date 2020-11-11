using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeMode
{
    IN,
    OUT
}

public class TextFader : MonoBehaviour
{
    // Public params
    public float fadeDuration;
    public FadeMode fadeMode;

    // Callbacks
    private Action OnFadeIn;

    // Cache
    private Text text;
    private float baseAlpha;

    // Inner
    private bool fading;
    private float timePassed;

    // Start is called before the first frame update
    void Start()
    {
        // Cache
        text = GetComponent<Text>();
        baseAlpha = text.color.a;

        // Inner
        fading = true;
        timePassed = 0;
        OnFadeIn = null;

        // Init
        SetTextAlpha(text, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(fading)
        {
            timePassed += Time.deltaTime;
            float ratio = Mathf.Min(timePassed / fadeDuration, 1f);
            float currentAlpha = fadeMode == FadeMode.IN ?  baseAlpha * ratio : baseAlpha * (1 - ratio);
            SetTextAlpha(text, currentAlpha);

            if (ratio >= 1f)
            {
                fading = false;
                OnFadeIn?.Invoke();
            }

        }
    }

    private void SetTextAlpha(Text text, float alpha)
    {
        Color currentColor = text.color;
        currentColor.a = alpha;
        text.color = currentColor;
    }

    public void SetOnFadeIn(Action OnFadeIn)
    {
        this.OnFadeIn = OnFadeIn;
    }

    public void Fade()
    {
        fading = true;
    }

    public void ResetFade()
    {
        SetTextAlpha(text, 0f);
        fading = false;
    }
}
