using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Transform cameraPosition;
    void Start()
    {
        cameraPosition = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, cameraPosition.eulerAngles.y, 0),1000);
    }
}
