using System;
using System.Collections;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class HealingItem : MonoBehaviour
{
    Material healingMaterial;
    GameObject HealingModel;
    BoxCollider boxCollider;
    PlayerController playerController;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            SkinnedMeshRenderer renderer = other.GetComponentInChildren<SkinnedMeshRenderer>();

            healingMaterial = renderer.materials[1];
            StartCoroutine(SetBoolForTime(healingMaterial, 1f));
            playerController.currentHealth += 30;
        }
    }

    IEnumerator SetBoolForTime(Material healingMaterial, float time)
    {
        var component = GetComponentInChildren<MeshRenderer>();
        Destroy(component.gameObject);
        healingMaterial.SetInt("_PickedUp", 1);
        yield return new WaitForSeconds(time);
        healingMaterial.SetInt("_PickedUp", 0);
    }
}
