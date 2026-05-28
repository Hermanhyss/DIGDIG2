using UnityEngine;
using UnityEngine.UI;
using Enemies;

public class EnemyHpBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Enemy enemy;

    [Header("Settings")]
    [SerializeField] private bool autoFindEnemy = true;
    [SerializeField] private bool faceCamera = true;

    private Camera mainCamera;

    void Start()
    {
        if (autoFindEnemy && enemy == null)
        {
            enemy = GetComponentInParent<Enemy>();
            if (enemy == null)
            {
                Debug.LogWarning("EnemyHpBar: Could not find Enemy component in parent!");
            }
        }

        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
            if (healthSlider == null)
            {
                Debug.LogWarning("EnemyHpBar: Could not find Slider component!");
            }
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("EnemyHpBar: Main camera not found!");
        }
    }

    void Update()
    {
        if (enemy != null && healthSlider != null)
        {
            float maxHealth = enemy.maxHealth;
            float currentHealth = GetEnemyCurrentHealth();

            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;

            // Dölj HP-baren om enemy är död
            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        if (faceCamera && mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.forward);
        }
    }

    private float GetEnemyCurrentHealth()
    {
        var healthField = typeof(Enemy).GetField("currentHealth", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (healthField != null)
        {
            return (float)healthField.GetValue(enemy);
        }

        return 0f;
    }
}
