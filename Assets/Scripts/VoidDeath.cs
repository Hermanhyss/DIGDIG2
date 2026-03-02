using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidDeath : MonoBehaviour
{

    UIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.ShowGameOverCanvas();
            
        }
    }
}
