using System;
using System.Collections;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class HealingItem : MonoBehaviour
{
    Material healingMaterial;
    [SerializeField] GameObject healingVfx;
    BoxCollider boxCollider;
    PlayerController playerController;
    int healNumber = 30;

    private void Start()
    {
        healingVfx = transform.Find("vfx_Heal_01").gameObject;
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
            playerController.currentHealth += healNumber;
            healingVfx.SetActive(true);
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
