using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticVignettePulse : MonoBehaviour
{
    Volume volume;
    [SerializeField] float pulseIntensity = 1f;
    [SerializeField] float vignetteStrength = 0.4f;
    [SerializeField] float duration = 0.3f;

    ChromaticAberration chroma;
    Vignette vignette;

    void Start()
    {
        volume = GetComponent<Volume>();
        if (volume.profile.TryGet(out chroma))
        {
            chroma.active = true;
            chroma.intensity.overrideState = true;
            chroma.intensity.value = 0f;
        }

        if (volume.profile.TryGet(out vignette))
        {
            vignette.active = true;
            vignette.intensity.overrideState = true;
            vignette.intensity.value = 0f;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(Pulse());
    }

    IEnumerator Pulse()
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            float curve = Mathf.Sin(t / duration * Mathf.PI);
            chroma.intensity.value = curve * pulseIntensity;
            vignette.intensity.value = curve * vignetteStrength;
            yield return null;
        }
        chroma.intensity.value = 0f;
        vignette.intensity.value = 0f;
    }
}
