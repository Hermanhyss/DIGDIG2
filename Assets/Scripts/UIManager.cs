using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenuCanvas;
    [SerializeField] GameObject quitMenuCanvas;
    [SerializeField] GameObject Blur;
    bool paused;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            paused = !paused;
            Debug.Log("Pressed escape");
        }

        if (paused)
        {
            pauseMenu.SetActive(true);
            Blur.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.SetActive(false);
            Blur.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void EnterContinue()
    {
        paused = !paused;
        Debug.Log("Continued Game");
    }

    public void EnterOptions()
    {
        paused = false;
        optionsMenuCanvas.SetActive(true);
        Debug.Log("Entered Options");
    }
    public void CloseOptions()
    {
        optionsMenuCanvas.SetActive(false);
        paused = true;
    }
    //Quit
    public void EnterQuit()
    {
        quitMenuCanvas.SetActive(true);
        paused = false;
        Debug.Log("Entered Quit");
    }

    public void CloseQuit()
    {
        quitMenuCanvas.SetActive(false);
        paused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
