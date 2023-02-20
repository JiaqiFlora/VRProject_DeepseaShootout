using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen instance { get; private set; }

    public bool fadeOnStart = true;
    public float fadeDuration = 2;
    public Color fadeColor;

    private Renderer rend;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();

        if(fadeOnStart)
        {
            FadeIn();
        }
    }


    public void FadeIn(float fadeDuration = 2f)
    {
        this.fadeDuration = fadeDuration;
        Fade(1, 0);
    }

    public void FadeOut(float fadeDuration = 2f)
    {
        this.fadeDuration = fadeDuration;
        Fade(0, 1);
    }

    private void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while(timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            rend.material.color = newColor;

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;

        rend.material.color = newColor2;
    }
}
