using UnityEngine;

public class FadeTrigger : MonoBehaviour
{
    public Animator fadeAnimator;
    public Transform teleportTarget;
    public GameObject player;
    public float fadeDuration = 1f; // Match your animation length

    private bool isFading = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isFading && other.CompareTag("Player"))
        {
            StartCoroutine(FadeAndTeleport());
        }
    }

    private System.Collections.IEnumerator FadeAndTeleport()
    {
        isFading = true;

        // Play FadeOut
        fadeAnimator.Play("FadeOut");

        // Wait for fade to black
        yield return new WaitForSeconds(fadeDuration);

        // Teleport the player
        player.transform.position = teleportTarget.position;

        // Fade back in
        fadeAnimator.Play("FadeIn");

        yield return new WaitForSeconds(fadeDuration);

        isFading = false;
    }
}
