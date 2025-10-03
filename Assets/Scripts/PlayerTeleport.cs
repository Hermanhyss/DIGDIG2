using Unity.Cinemachine;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] CinemachineCamera aCam;
    public void Teleport(Vector3 pos)
    {
        // mörda kameran, ingen lerp
        //corotuionre? 1 sek
        transform.position = pos;
        aCam.transform.position = pos;
        //start kameran igen
    }
}
