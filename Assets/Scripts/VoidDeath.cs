using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidDeath : MonoBehaviour
{
    UIManager uiManager;
    PlayerController player;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        player = FindObjectOfType<PlayerController>();
    }   
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.Respawn();
            
            
        }
    }
}
