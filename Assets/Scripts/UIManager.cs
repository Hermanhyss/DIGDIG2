using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenuCanvas;
    [SerializeField] GameObject quitMenuCanvas;

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
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void EnterOptions()
    {
        optionsMenuCanvas.SetActive(true);
        pauseMenu.SetActive(false);
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
