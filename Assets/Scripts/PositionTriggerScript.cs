using UnityEngine;

public class PositionTriggerScript : MonoBehaviour
{
    [SerializeField]GameObject player;
    PlayerTeleport pt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone.");
            // Add your logic here for when the player enters the trigger zone
        }

        pt = player.GetComponent<PlayerTeleport>();
        pt.Teleport(Vector3.zero);
    }

}
