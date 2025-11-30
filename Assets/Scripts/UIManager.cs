using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

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

    public void EnterOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenuCanvas.SetActive(true);
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
