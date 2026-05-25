using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    [Header("Menu UI")]
    public GameObject mainMenuUI;
    public GameObject settingsMenuUI;

    [Header("Buttons")]
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;
    public Button backButton;

    [Header("Button Sprites")]
    public Sprite startNormal;
    public Sprite startHover;
    public Sprite startClicked;
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
    public GameObject startArrow;
    public GameObject settingsArrow;
    public GameObject quitArrow;
    public GameObject backArrow;

    [Header("Arrow Animation Settings")]
    public float arrowMoveDistance = 10f;
    public float arrowAnimationSpeed = 3f;

    [Header("Click Settings")]
    public float clickDelay = 0.15f;

    // Arrow animation tracking
    private bool animateStartArrow = false;
    private bool animateSettingsArrow = false;
    private bool animateQuitArrow = false;
    private bool animateBackArrow = false;

    private Vector3 startArrowStartPos;
    private Vector3 settingsArrowStartPos;
    private Vector3 quitArrowStartPos;
    private Vector3 backArrowStartPos;

    private float arrowAnimTime = 0f;

    void Start()
    {
        if (mainMenuUI != null)
            mainMenuUI.SetActive(true);
        if (settingsMenuUI != null)
            settingsMenuUI.SetActive(false);

        // Store arrow starting positions
        if (startArrow != null)
        {
            startArrowStartPos = startArrow.transform.localPosition;
            startArrow.SetActive(false);
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

        SetupButtonHover(startButton, startNormal, startHover);
        SetupButtonHover(settingsButton, settingsNormal, settingsHover);
        SetupButtonHover(quitButton, quitNormal, quitHover);
        SetupButtonHover(backButton, backNormal, backHover);
    }

    void Update()
    {
        // Animate arrows
        arrowAnimTime += Time.deltaTime * arrowAnimationSpeed;
        float offset = Mathf.Sin(arrowAnimTime) * arrowMoveDistance;

        if (animateStartArrow && startArrow != null && startArrow.activeSelf)
        {
            startArrow.transform.localPosition = startArrowStartPos + new Vector3(offset, 0, 0);
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
        if (button == startButton && startArrow != null)
        {
            startArrow.SetActive(isHovering);
            animateStartArrow = isHovering;
            if (isHovering)
                startArrow.transform.localPosition = startArrowStartPos;
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
            if (button == startButton && startClicked != null)
                buttonImage.sprite = startClicked;
            else if (button == settingsButton && settingsClicked != null)
                buttonImage.sprite = settingsClicked;
            else if (button == quitButton && quitClicked != null)
                buttonImage.sprite = quitClicked;
            else if (button == backButton && backClicked != null)
                buttonImage.sprite = backClicked;
        }

        // Hide arrow during click
        if (button == startButton && startArrow != null)
        {
            startArrow.SetActive(false);
            animateStartArrow = false;
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
        if (button == startButton)
            StartCoroutine(ExecuteAfterDelay(() => StartGame()));
        else if (button == settingsButton)
            StartCoroutine(ExecuteAfterDelay(() => Settings()));
        else if (button == quitButton)
            StartCoroutine(ExecuteAfterDelay(() => Quit()));
        else if (button == backButton)
            StartCoroutine(ExecuteAfterDelay(() => GoBackToMainMenu()));
    }

    IEnumerator ExecuteAfterDelay(System.Action action)
    {
        yield return new WaitForSeconds(clickDelay);
        action?.Invoke();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 1");
    }

    public void Settings()
    {
        if (mainMenuUI != null)
            mainMenuUI.SetActive(false);
        if (settingsMenuUI != null)
            settingsMenuUI.SetActive(true);

        // Re-enable buttons
        if (startButton != null) startButton.interactable = true;
        if (settingsButton != null) settingsButton.interactable = true;
        if (quitButton != null) quitButton.interactable = true;
        if (backButton != null) backButton.interactable = true;
    }

    public void GoBackToMainMenu()
    {
        if (settingsMenuUI != null)
            settingsMenuUI.SetActive(false);
        if (mainMenuUI != null)
            mainMenuUI.SetActive(true);

        // Re-enable buttons
        if (startButton != null) startButton.interactable = true;
        if (settingsButton != null) settingsButton.interactable = true;
        if (quitButton != null) quitButton.interactable = true;
        if (backButton != null) backButton.interactable = true;

        // Reset button sprites to normal state
        ResetButtonSprites();
    }

    void ResetButtonSprites()
    {
        // Reset start button
        if (startButton != null && startNormal != null)
        {
            Image buttonImage = startButton.GetComponent<Image>();
            if (buttonImage != null)
                buttonImage.sprite = startNormal;
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
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
