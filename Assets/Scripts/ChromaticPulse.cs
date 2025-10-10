using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticPulse : MonoBehaviour
{
    Volume volume;
    [SerializeField] float pulseIntensity = 1f;
    [SerializeField] float duration = 0.3f;

    ChromaticAberration chroma;

    void Start()
    {
        volume = GetComponent<Volume>();

        // Get the Chromatic Aberration override from the volume profile
        if (volume.profile.TryGet<ChromaticAberration>(out chroma))
        {
            // Make sure override is enabled
            chroma.active = true;
            chroma.intensity.overrideState = true;
            chroma.intensity.value = 0f;
        }
        else
        {
            Debug.LogError("Chromatic Aberration override not found in volume profile");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Pulse());
        }
    }

    IEnumerator Pulse()
    {
        float elapsed = 0f;
        float startValue = chroma.intensity.value;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Use a smooth curve (sin) for pulsing
            float value = Mathf.Sin(t * Mathf.PI) * pulseIntensity;
            chroma.intensity.value = value;
            yield return null;
        }

        chroma.intensity.value = 0f;
    }
}
