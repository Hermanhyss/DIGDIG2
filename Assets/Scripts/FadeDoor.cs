using UnityEngine;
using UnityEngine.UI;

public class FadeDoor : MonoBehaviour
{
    public Image fadeImage; // Assign a UI Image (full screen, black, alpha 0) in the Inspector
    public float fadeDuration = 1f;
    public float fadeHoldDuration = 1f; // How long to stay fully faded

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeSequence());
        }
    }

    private System.Collections.IEnumerator FadeSequence()
    {
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(fadeHoldDuration);
        yield return StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color color = fadeImage.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1f;
        fadeImage.color = color;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color color = fadeImage.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0f;
        fadeImage.color = color;
    }
}
