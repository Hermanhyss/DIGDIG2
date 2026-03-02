using UnityEngine;
using UnityEngine.UI;

public class DoorEnding : MonoBehaviour
{
    [Header("UI")]
    public Image fadeImage;                 // Assign a full-screen black Image (alpha 0)
    public GameObject interactPrompt;       // Assign a hidden "Press E" UI GameObject
    public GameObject endingCanvas;         // Assign your "Continuing" Canvas here
    public KeyCode interactKey = KeyCode.E;

    [Header("Fade")]
    public float fadeDuration = 1f;
    public float fadeHoldDuration = 1f;     

    private GameObject _triggeredPlayer;
    private bool _playerInZone;
    private bool _isFading;

    private Animator animator;
    private AudioSource audioSource;
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (endingCanvas != null)
            endingCanvas.SetActive(false);
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

        
        if (endingCanvas != null)
            endingCanvas.SetActive(true);

      
        yield return new WaitForSeconds(fadeHoldDuration);

       Time.timeScale = 0f; // Pause the game
        // yield return StartCoroutine(FadeOut());

        _isFading = false;
    }

    private System.Collections.IEnumerator FadeIn()
    {
        PlaySound();
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

    //private System.Collections.IEnumerator FadeOut()
    //{
    //    animator.SetBool("DoorOpen", false);
    //    float elapsed = 0f;
    //    Color color = fadeImage.color;
    //    while (elapsed < fadeDuration)
    //    {
    //        elapsed += Time.deltaTime;
    //        color.a = 1f - Mathf.Clamp01(elapsed / fadeDuration);
    //        fadeImage.color = color;
    //        yield return null;
    //    }
    //    color.a = 0f;
    //    fadeImage.color = color;
    //}

    private void PlaySound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
