using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image blackScreen;
    public float fadeDuration = 1.0f;

    private bool isFading = false;

    public void FadeIn(Action onComplete = null)
    {
        if (!isFading)
        {
            StartCoroutine(FadeToColor(blackScreen, 1, fadeDuration, onComplete));
        }
    }

    public void FadeOut(Action onComplete = null)
    {
        if (!isFading)
        {
            StartCoroutine(FadeToColor(blackScreen, 0, fadeDuration, onComplete));
        }
    }

    IEnumerator FadeToColor(Image image, float targetAlpha, float duration, Action onComplete)
    {
        isFading = true;
        Color currentColor = image.color;
        Color targetColor = new(currentColor.r, currentColor.g, currentColor.b, targetAlpha);

        float timer = 0;

        while (timer < duration)
        {
            image.color = Color.Lerp(currentColor, targetColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor;
        isFading = false;
        onComplete?.Invoke();
    }
}
