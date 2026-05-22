using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool buttonPressed;
    private AudioManagerNew audioManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        audioManager.PlaySFX(0);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}