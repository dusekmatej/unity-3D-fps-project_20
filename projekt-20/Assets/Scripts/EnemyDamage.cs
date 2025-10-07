using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage & Detection Settings")]
    public float attackRange = 3f;
    public float attackCooldown = 1f;
    public int damageAmount = 10;
    public TMP_Text healthDisplay;
    public GameObject deathScreenUI; 
    public Button restartButton; 

    private float lastAttackTime = 0f;
    private Transform playerTransform;
    private int playerHealth = 100;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (playerTransform == null)
        {
            Debug.LogError("Unable to find player object. Ensure the player GameObject has the tag 'Player'.");
        }

        if (healthDisplay == null)
        {
            Debug.LogError("HealthDisplay is not assigned to the script. Assign it via the Inspector.");
        }

        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame); 
        }
        else
        {
            Debug.LogError("Restart Button is not assigned in the Inspector.");
        }

        UpdateHealthDisplay();
    }

    private void Update()
    {
        if (playerTransform == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            // Perform a raycast to check if the player is visible (no walls between them)
            RaycastHit hit;
            if (Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit, attackRange))
            {
                if (hit.collider.CompareTag("Player") && Time.time - lastAttackTime >= attackCooldown)
                {
                    DealDamageToPlayer();
                    lastAttackTime = Time.time;
                }
            }
        }
    }


    private void DealDamageToPlayer()
    {
        playerHealth -= damageAmount;
        UpdateHealthDisplay();

        if (playerHealth <= 0)
        {
            PlayerDies();
        }
    }

    private void UpdateHealthDisplay()
    {
        if (healthDisplay != null)
        {
            healthDisplay.text = $"Health: {playerHealth}";
        }
    }

    private void PlayerDies()
    {
        Debug.Log("Player has died. Showing death screen...");

        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);
        }

        Time.timeScale = 0f;

        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
