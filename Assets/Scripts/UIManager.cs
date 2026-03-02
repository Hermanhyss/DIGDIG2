using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.UI;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenuCanvas;
    [SerializeField] GameObject quitMenuCanvas;
    [SerializeField] GameObject gameOverCanvas;
    //[SerializeField] GameObject Blur;
    bool paused;
    bool gameOver;
    bool pressedEscape;

    [SerializeField] List<Image> buttonsImages;
    [SerializeField] Sprite originalButtonImage;

    private void Start()
    {
        gameOverCanvas.SetActive(false);
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
            //Blur.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.SetActive(false);
            //Blur.SetActive(false);
            Time.timeScale = 1f;
        }

        
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
        //Time.timeScale = 1f;
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        //Debug.Log("Entered Main Menu");
    }

    public void EnterOptions()
    {
        paused = true;
        optionsMenuCanvas.SetActive(true);
        pauseMenu.SetActive(false);
        Debug.Log("Entered Options");
    }
    public void CloseOptions()
    {
        optionsMenuCanvas.SetActive(false);
        pauseMenu.SetActive(true);



    }
    //Quit
    public void EnterQuit()
    {
        quitMenuCanvas.SetActive(true);
        pauseMenu.SetActive(false);
        Debug.Log("Entered Quit");
    }


    public void CloseQuit()
    {
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

    public void ShowGameOverCanvas()
    {
        //Time.timeScale = 1f;
        gameOverCanvas.SetActive(false);
        
        
        
        
 
        Debug.Log("Restarted Scene");
    }

    public void QuitGame()
    {

        Application.Quit();
    }
}
