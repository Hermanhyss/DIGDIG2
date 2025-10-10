using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float width;
    [SerializeField] private float height;

    [Header("UI Reference")]
    [SerializeField]
    private RectTransform healthBar;
    public float Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            UpdateBar();
        }
    }
    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = Mathf.Max(1, value);
            health = Mathf.Clamp(health, 0, maxHealth);
            UpdateBar();
        }
    }
    private void OnValidate()
    {
        Health = health;
        MaxHealth = maxHealth;
        UpdateBar();
    }
    public void SetMaxHealth(float value)
    {
        MaxHealth = value;
    }
    public void SetHealth(float value)
    {
        Health = value;
    }
    private void UpdateBar()
    {
        if (healthBar != null && maxHealth > 0)
        {
            float newWidth = (health / maxHealth) * width;
            healthBar.sizeDelta = new Vector2(newWidth, height);
        }
    }
}
