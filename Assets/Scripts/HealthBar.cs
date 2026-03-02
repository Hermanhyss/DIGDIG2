using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float width;
    [SerializeField] private float height;

    [Header("UI Reference")]
    [SerializeField]
    private RectTransform healthBar;

    private PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        UpdateBar();
    }

    private void Update()
    {
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (playerController != null && healthBar != null && playerController.maxHealth > 0)
        {
            float newWidth = (playerController.CurrentHealth / playerController.maxHealth) * width;
            healthBar.sizeDelta = new Vector2(newWidth, height);
        }
    }
}
