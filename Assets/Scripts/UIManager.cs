using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenuCanvas;
    [SerializeField] GameObject quitMenuCanvas;
    [SerializeField] GameObject gameOverCanvas;

    [SerializeField] GameObject indicators;
    [SerializeField] GameObject buttonIndicatorPlay;
    [SerializeField] GameObject buttonIndicatorOptions;
    [SerializeField] GameObject buttonIndicatorQuit;

    bool paused;
    bool gameOver;
    bool pressedEscape;
    bool menuOpen;

    [SerializeField] List<Image> buttonsImages;
    [SerializeField] Sprite originalButtonImage;

    public Animator playAnim;
    public Animator optionsAnim;
    public Animator quitAnim;

    public AnimationClip playAnimClip;
    public AnimationClip optionsAnimClip;
    public AnimationClip quitAnimClip;


    private void Start()
    {
        gameOverCanvas.SetActive(false); // Oscar Har varit här
        optionsMenuCanvas.SetActive(false);// Oscar Har varit här
        quitMenuCanvas.SetActive(false);// Oscar Har varit här
        pauseMenu.SetActive(false);// Oscar Har varit här

        buttonIndicatorPlay.SetActive(false);
        buttonIndicatorOptions.SetActive(false);
        buttonIndicatorQuit.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            paused = !paused;
            Debug.Log("Pressed escape");

            pauseMenu.SetActive(true);

            foreach (Image buttonImage in buttonsImages)
            {
                buttonImage.sprite = originalButtonImage;
            }

            if(optionsMenuCanvas.activeSelf)
            {
                optionsMenuCanvas.SetActive(false);
            }

            if (quitMenuCanvas.activeSelf)
            {
                quitMenuCanvas.SetActive(false);
            }

        }

        if (paused)
        {           
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        //if (menuOpen)
        //{
        //    buttonIndicatorOptions.SetActive(false);
        //    buttonIndicatorQuit.SetActive(false);
        //}
        //else if (!menuOpen)
        //{
        //    buttonIndicatorOptions.SetActive(true);
        //    buttonIndicatorQuit.SetActive(true);
        //}
    }

    public void PlayHoverEnter()
    {
        playAnimClip.frameRate = 60f;
        buttonIndicatorPlay.SetActive(true);

    }

    public void PlayHoverExit()
    {
        buttonIndicatorPlay.SetActive(false);
    }

    public void OptionsHoverEnter()
    {
        optionsAnimClip.frameRate = 60f;
        buttonIndicatorOptions.SetActive(true);
    }

    public void OptionsHoverExit()
    {
        buttonIndicatorOptions.SetActive(false);
    }

    public void QuitHoverEnter()
    {
        quitAnimClip.frameRate = 60f;
        buttonIndicatorQuit.SetActive(true);
    }

    public void QuitHoverExit()
    {
        buttonIndicatorQuit.SetActive(false);
    }

    public void EnterNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Entered Next Level");
    }

    public void EnterContinue()
    {  
        paused = false;
        Time.timeScale = 1f;
        if(pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
            

        Debug.Log("Continued Game");
    }

    public void EnterMainMenu()
    {
        paused = !paused;

        if(pauseMenu.activeSelf == false)
        {
            pauseMenu.SetActive(true);
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

        //Debug.Log("Entered Main Menu");
    }

    public void EnterOptions()
    {
        //paused = true;
        optionsMenuCanvas.SetActive(true);
        pauseMenu.SetActive(false);
        Debug.Log("Entered Options");
    }
    public void CloseOptions()
    {
        menuOpen = false;
        optionsMenuCanvas.SetActive(false);
        pauseMenu.SetActive(true);
        Debug.Log("Closed Options");
    }
    
    public void EnterQuit()
    {
        menuOpen = true;
        quitMenuCanvas.SetActive(true);
        pauseMenu.SetActive(false);
        Debug.Log("Entered Quit");
    }


    public void CloseQuit()
    {
        menuOpen = false;
        quitMenuCanvas.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void TryAgain()
    {
        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Restarted Scene");
    }

    public void ShowGameOverCanvas() // Oscar jobbar på denna // Färdigt
    {    
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {

        Application.Quit();
        Debug.Log("Quit Game");
    }
}
