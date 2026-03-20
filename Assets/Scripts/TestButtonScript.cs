using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public bool buttonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        FindAnyObjectByType<AudioManagerNew>().PlaySound(1);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}