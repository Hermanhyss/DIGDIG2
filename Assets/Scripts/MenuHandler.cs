using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEditor;

public class MenuHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject optionsMenuCanvas;
    [SerializeField] GameObject quitMenuCanvas;
    //[SerializeField] AudioClip clickSound;



    Animator animator;
    AudioSource myAudioSource;

    //public bool isPaused;
    //public bool GetIsPaused() { return isPaused; }

    //[SerializeField] AudioSource buttonPress;

    void Start()
    {

    }
    //Options

    
    public void Awake()
    {
        //DontDestroyOnLoad(clickSound);
        
    }
    //Coroutine

    void Update()
    {
        
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
        } 
    }
    public void Resume()
    {
        //Gör så att spelet slutar vara pausat
        //Ta bort blurriga bakrunden
    }


    //public void PlayClickSound()
    //{
    //    GetComponent<AudioSource>().Play();
    //}
    public void EnterOptions()
    {
        optionsMenuCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }
    public void CloseOptions()
    {
        optionsMenuCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
    //Quit
    public void EnterQuit()
    {
        quitMenuCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);
    }

    public void CloseQuit()
    {
        quitMenuCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
