using TMPro;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int _currentHealth;

    public TMP_Text healthDisplay;
    
    void Awake()
    {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        _currentHealth = Mathf.Max(_currentHealth, 0);

        UpdateUI();
    }

    public bool IsDead()
    {
        return _currentHealth <= 0;
    }

    public void UpdateUI()
    {
        if (healthDisplay != null)
        {
            healthDisplay.text = $"Health: {_currentHealth}";
        }
    }
    
    public int GetCurrentHealth() => _currentHealth;
    public int GetMaxHealth() => maxHealth;
}
