using System;
using System.Collections;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    Material healingMaterial;
    int healBoolValue = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SkinnedMeshRenderer renderer = other.GetComponentInChildren<SkinnedMeshRenderer>();

            healingMaterial = renderer.materials[1];
            Debug.Log("material get");
            StartCoroutine(SetBoolForTime(healingMaterial, 1, 2f));
            Debug.Log("Player has interacted with healing");
        }
    }

    IEnumerator SetBoolForTime(Material healingMaterial, int healBoolValue, float time)
    {
        Debug.Log("coroutine started");
        healingMaterial.SetInt("_PickedUp", 1);
        yield return new WaitForSeconds(time);
        healingMaterial.SetInt("_PickedUp", 0);
    }
}
