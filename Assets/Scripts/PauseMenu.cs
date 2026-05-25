using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    [Header("Menu UI")]
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    [Header("Buttons")]
    public Button continueButton;
    public Button settingsButton;
    public Button quitButton;
    public Button backButton;

    [Header("Button Sprites")]
    public Sprite continueNormal;
    public Sprite continueHover;
    public Sprite continueClicked;
    public Sprite settingsNormal;
    public Sprite settingsHover;
    public Sprite settingsClicked;
    public Sprite quitNormal;
    public Sprite quitHover;
    public Sprite quitClicked;
    public Sprite backNormal;
    public Sprite backHover;
    public Sprite backClicked;

    [Header("Arrow Indicators")]
    public GameObject continueArrow;
    public GameObject settingsArrow;
    public GameObject quitArrow;
    public GameObject backArrow;

    [Header("Arrow Animation Settings")]
    public float arrowMoveDistance = 10f;
    public float arrowAnimationSpeed = 3f;

    [Header("Click Settings")]
    public float clickDelay = 0.15f;

    private bool isPaused = false;

    // Arrow animation tracking
    private bool animateContinueArrow = false;
    private bool animateSettingsArrow = false;
    private bool animateQuitArrow = false;
    private bool animateBackArrow = false;

    private Vector3 continueArrowStartPos;
    private Vector3 settingsArrowStartPos;
    private Vector3 quitArrowStartPos;
    private Vector3 backArrowStartPos;

    private float arrowAnimTime = 0f;

    void Start()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        if (settingsMenuUI != null)
            settingsMenuUI.SetActive(false);

        // Store arrow starting positions
        if (continueArrow != null)
        {
            continueArrowStartPos = continueArrow.transform.localPosition;
            continueArrow.SetActive(false);
        }
        if (settingsArrow != null)
        {
            settingsArrowStartPos = settingsArrow.transform.localPosition;
            settingsArrow.SetActive(false);
        }
        if (quitArrow != null)
        {
            quitArrowStartPos = quitArrow.transform.localPosition;
            quitArrow.SetActive(false);
        }
        if (backArrow != null)
        {
            backArrowStartPos = backArrow.transform.localPosition;
            backArrow.SetActive(false);
        }

        SetupButtonHover(continueButton, continueNormal, continueHover);
        SetupButtonHover(settingsButton, settingsNormal, settingsHover);
        SetupButtonHover(quitButton, quitNormal, quitHover);
        SetupButtonHover(backButton, backNormal, backHover);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Continue();
            else
                Pause();
        }

        // Animate arrows using unscaled time (works when paused!)
        arrowAnimTime += Time.unscaledDeltaTime * arrowAnimationSpeed;
        float offset = Mathf.Sin(arrowAnimTime) * arrowMoveDistance;

        if (animateContinueArrow && continueArrow != null && continueArrow.activeSelf)
        {
            continueArrow.transform.localPosition = continueArrowStartPos + new Vector3(offset, 0, 0);
        }
        if (animateSettingsArrow && settingsArrow != null && settingsArrow.activeSelf)
        {
            settingsArrow.transform.localPosition = settingsArrowStartPos + new Vector3(offset, 0, 0);
        }
        if (animateQuitArrow && quitArrow != null && quitArrow.activeSelf)
        {
            quitArrow.transform.localPosition = quitArrowStartPos + new Vector3(offset, 0, 0);
        }
        if (animateBackArrow && backArrow != null && backArrow.activeSelf)
        {
            backArrow.transform.localPosition = backArrowStartPos + new Vector3(offset, 0, 0);
        }
    }

    void SetupButtonHover(Button button, Sprite normalSprite, Sprite hoverSprite)
    {
        if (button == null) return;

        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((data) => 
        { 
            OnButtonHover(button, hoverSprite, true); 
        });
        trigger.triggers.Add(pointerEnter);

        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((data) => 
        { 
            OnButtonHover(button, normalSprite, false); 
        });
        trigger.triggers.Add(pointerExit);

        // Add click handler
        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((data) => 
        { 
            OnButtonClick(button); 
        });
        trigger.triggers.Add(pointerDown);
    }

    void OnButtonHover(Button button, Sprite sprite, bool isHovering)
    {
        // Change button sprite
        if (sprite != null)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
                buttonImage.sprite = sprite;
        }

        // Handle arrows
        if (button == continueButton && continueArrow != null)
        {
            continueArrow.SetActive(isHovering);
            animateContinueArrow = isHovering;
            if (isHovering)
                continueArrow.transform.localPosition = continueArrowStartPos;
        }
        else if (button == settingsButton && settingsArrow != null)
        {
            settingsArrow.SetActive(isHovering);
            animateSettingsArrow = isHovering;
            if (isHovering)
                settingsArrow.transform.localPosition = settingsArrowStartPos;
        }
        else if (button == quitButton && quitArrow != null)
        {
            quitArrow.SetActive(isHovering);
            animateQuitArrow = isHovering;
            if (isHovering)
                quitArrow.transform.localPosition = quitArrowStartPos;
        }
        else if (button == backButton && backArrow != null)
        {
            backArrow.SetActive(isHovering);
            animateBackArrow = isHovering;
            if (isHovering)
                backArrow.transform.localPosition = backArrowStartPos;
        }

        // Reset animation time when starting hover
        if (isHovering)
            arrowAnimTime = 0f;
    }

    void OnButtonClick(Button button)
    {
        // Show clicked sprite
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            if (button == continueButton && continueClicked != null)
                buttonImage.sprite = continueClicked;
            else if (button == settingsButton && settingsClicked != null)
                buttonImage.sprite = settingsClicked;
            else if (button == quitButton && quitClicked != null)
                buttonImage.sprite = quitClicked;
            else if (button == backButton && backClicked != null)
                buttonImage.sprite = backClicked;
        }

        // Hide arrow during click
        if (button == continueButton && continueArrow != null)
        {
            continueArrow.SetActive(false);
            animateContinueArrow = false;
        }
        else if (button == settingsButton && settingsArrow != null)
        {
            settingsArrow.SetActive(false);
            animateSettingsArrow = false;
        }
        else if (button == quitButton && quitArrow != null)
        {
            quitArrow.SetActive(false);
            animateQuitArrow = false;
        }
        else if (button == backButton && backArrow != null)
        {
            backArrow.SetActive(false);
            animateBackArrow = false;
        }

        // Disable button to prevent double clicks
        button.interactable = false;

        // Execute action after delay
        if (button == continueButton)
            StartCoroutine(ExecuteAfterDelay(() => Continue()));
        else if (button == settingsButton)
            StartCoroutine(ExecuteAfterDelay(() => Settings()));
        else if (button == quitButton)
            StartCoroutine(ExecuteAfterDelay(() => Quit()));
        else if (button == backButton)
            StartCoroutine(ExecuteAfterDelay(() => GoBackToPauseMenu()));
    }

    IEnumerator ExecuteAfterDelay(System.Action action)
    {
        yield return new WaitForSecondsRealtime(clickDelay);
        action?.Invoke();
    }

    public void Pause()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Re-enable buttons when opening menu
        if (continueButton != null) continueButton.interactable = true;
        if (settingsButton != null) settingsButton.interactable = true;
        if (quitButton != null) quitButton.interactable = true;
        if (backButton != null) backButton.interactable = true;

        // Reset button sprites to normal state
        ResetButtonSprites();
    }

    public void Continue()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        if (settingsMenuUI != null)
            settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Re-enable buttons for next time
        if (continueButton != null) continueButton.interactable = true;
        if (settingsButton != null) settingsButton.interactable = true;
        if (quitButton != null) quitButton.interactable = true;
        if (backButton != null) backButton.interactable = true;

        // Reset button sprites to normal state
        ResetButtonSprites();
    }

    public void Settings()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        if (settingsMenuUI != null)
            settingsMenuUI.SetActive(true);

        // Re-enable buttons
        if (continueButton != null) continueButton.interactable = true;
        if (settingsButton != null) settingsButton.interactable = true;
        if (quitButton != null) quitButton.interactable = true;
        if (backButton != null) backButton.interactable = true;
    }

    public void GoBackToPauseMenu()
    {
        if (settingsMenuUI != null)
            settingsMenuUI.SetActive(false);
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        // Re-enable buttons
        if (continueButton != null) continueButton.interactable = true;
        if (settingsButton != null) settingsButton.interactable = true;
        if (quitButton != null) quitButton.interactable = true;
        if (backButton != null) backButton.interactable = true;

        // Reset button sprites to normal state
        ResetButtonSprites();
    }

    void ResetButtonSprites()
    {
        // Reset continue button
        if (continueButton != null && continueNormal != null)
        {
            Image buttonImage = continueButton.GetComponent<Image>();
            if (buttonImage != null)
                buttonImage.sprite = continueNormal;
        }

        // Reset settings button
        if (settingsButton != null && settingsNormal != null)
        {
            Image buttonImage = settingsButton.GetComponent<Image>();
            if (buttonImage != null)
                buttonImage.sprite = settingsNormal;
        }

        // Reset quit button
        if (quitButton != null && quitNormal != null)
        {
            Image buttonImage = quitButton.GetComponent<Image>();
            if (buttonImage != null)
                buttonImage.sprite = quitNormal;
        }

        // Reset back button
        if (backButton != null && backNormal != null)
        {
            Image buttonImage = backButton.GetComponent<Image>();
            if (buttonImage != null)
                buttonImage.sprite = backNormal;
        }
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
