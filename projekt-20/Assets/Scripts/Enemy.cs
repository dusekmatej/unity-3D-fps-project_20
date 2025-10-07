using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(1);
        }

        Destroy(gameObject);
    }
}