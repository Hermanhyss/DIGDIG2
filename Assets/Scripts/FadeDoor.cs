using UnityEngine;
using UnityEngine.UI;

public class FadeDoor : MonoBehaviour
{
    [Header("UI")]
    public Image fadeImage;                 // Assign a full-screen black Image (alpha 0)
    public GameObject interactPrompt;       // Assign a hidden "Press E" UI GameObject
    public KeyCode interactKey = KeyCode.E;

    [Header("Fade")]
    public float fadeDuration = 1f;
    public float fadeHoldDuration = 1f;     // How long to stay fully faded

    [Header("Teleport Settings")]
    public Transform teleportTarget;        // Assign the destination Transform
    public bool matchTargetRotation = true; // Rotate player to match target's rotation

    private GameObject _triggeredPlayer;
    private bool _playerInZone;
    private bool _isFading;

    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    private void Update()
    {
        if (_playerInZone && !_isFading && Input.GetKeyDown(interactKey))
        {
           

            if (interactPrompt != null)
                interactPrompt.SetActive(false);

            StartCoroutine(FadeSequence());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isFading) return;

        if (other.CompareTag("Player"))
        {
            _triggeredPlayer = other.gameObject;
            _playerInZone = true;

            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInZone = false;
            _triggeredPlayer = null;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }

    private System.Collections.IEnumerator FadeSequence()
    {
        _isFading = true;
       
        // Fade to black
        yield return StartCoroutine(FadeIn());

        // Teleport while fully faded
        if (_triggeredPlayer != null && teleportTarget != null)
        {
            var cc = _triggeredPlayer.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            _triggeredPlayer.transform.position = teleportTarget.position;
            if (matchTargetRotation)
            {
                _triggeredPlayer.transform.rotation = teleportTarget.rotation;
            }

            // Small safety step to clear potential collisions before enabling
            yield return null;

            if (cc != null) cc.enabled = true;
        }

        // Hold black screen for a moment
        yield return new WaitForSeconds(fadeHoldDuration);

        // Fade back in
        yield return StartCoroutine(FadeOut());

        // Interaction ends after teleport
        _isFading = false;
    }

    private System.Collections.IEnumerator FadeIn()
    {
        animator.SetBool("DoorOpen", true);
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
        animator.SetBool("DoorOpen", false);
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
