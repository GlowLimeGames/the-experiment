using System;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
public class Grayscale : ImageEffectBase
{
    public Texture textureRamp;

    // -1 = black, 1 = white
    [Range(-1.0f, 1.0f)]
    public float rampOffset;

    [Range(0, 1)]
    public float effectAmount;

    // Fades to black if toDark = true, else fades in from black
    public Coroutine Fade(bool toDark, float time, bool darkness = true)
    {
        return StartCoroutine(FadeCoroutine(toDark, time, darkness));
    }

    private IEnumerator FadeCoroutine(bool toDark, float time, bool darkness)
    {
        float initialRampOffset = toDark ? 0 : (darkness ? -1 : 1);
        float targetRampOffset = toDark ? (darkness ? -1 : 1) : 0;
        for (float t = 0, p = 0; t < time; t += Time.deltaTime, p = t / time)
        {
            rampOffset = Mathf.Lerp(initialRampOffset, targetRampOffset, p);
            effectAmount = toDark ? p : 1 - p;
            yield return null;
        }
    }

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetTexture("_RampTex", textureRamp);
        material.SetFloat("_RampOffset", rampOffset);
        material.SetFloat("_EffectAmount", effectAmount);
        Graphics.Blit(source, destination, material);
    }
}
