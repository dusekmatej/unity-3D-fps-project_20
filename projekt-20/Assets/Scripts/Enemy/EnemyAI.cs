using System;
using System.Threading;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    private float _timer;
    
    // [Header ("SOMETHING")]
    public NavMeshAgent navAgent;
    public Transform playerObject;
    public LayerMask groundLayer, playerLayer, obstacleLayer;

    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;
    public GameObject projectile;
    public float health;
    //public TMPro.TMP_Text inRangeText;
    //public TMPro.TMP_Text inSightText;

    public float atttackCooldown = 2f;
    private bool hasAttacked;

    public float detectRange, attackRange;

    private bool inRange;
    private bool inSight;

    private bool lastInSight;
    private bool lastInRange;
    
    
    void Awake()
    {
        playerObject = GameObject.FindWithTag("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (navAgent == null || playerObject == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerObject.position);

        inRange = distanceToPlayer <= attackRange;
        inSight = distanceToPlayer <= detectRange && PlayerVisible();



        bool withinDetectRange = distanceToPlayer <= detectRange;
        bool withinAttackRange = distanceToPlayer <= attackRange;

        bool hasSight = false;
        if (withinDetectRange && withinAttackRange)
        {
            Vector3 directionToPlayer = (playerObject.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, detectRange))
                hasSight = PlayerVisible();
        }


        inSight = distanceToPlayer <= detectRange;
        inRange = distanceToPlayer <= attackRange;

        //inSightText.text = $"In Sight: {inSight}";
        //inRangeText.text = $"In Range: {inRange}";

        // _timer += Time.deltaTime;
        // if (_timer >= 0.2f)
        // {
        //     _timer = 0f;
        //     Patrol();
        // }
        
        if (!inSight && !inRange) Patrol(); 
        if (inSight && !inRange) Follow();
        if (inRange && inSight) Attack();

        // if (inSightText != null && inSight != lastInSight)
        // {
        //     inSightText.SetText(inSight ? "In Sight: True" : "In Sight: False");
        //     lastInSight = inSight;
        // }
        //
        // if (inRangeText != null && inRange != lastInRange)
        // {
        //     inRangeText.SetText(inRange ? "In Range: True" : "In Range: False");
        //     lastInRange = inRange;
        // }
}

    private bool PlayerVisible()
    {
        Vector3 eyePosition = transform.position + Vector3.up * 1.5f + transform.forward * 0.1f;
        Vector3 targetPosition = playerObject.position + new Vector3(0, 1f, 0);
        Vector3 direction = (targetPosition - eyePosition).normalized;
        float distance = Vector3.Distance(eyePosition, targetPosition);

        if (Physics.Raycast(eyePosition, direction, out RaycastHit hit, distance, ~0))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }
    
    private void Patrol()
    {
        if (!walkPointSet) FindWalkPoint();

        if (walkPointSet)
        {
            navAgent.SetDestination(walkPoint);
        }
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        if (distanceToWalkPoint.magnitude < .1f)
        {
            walkPointSet = false;
        }
    }

    private void FindWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    
        Debug.DrawRay(walkPoint, Vector3.up * 5f, Color.red, 5f);
        
        Vector3 distanceToWalkPoint = walkPoint - transform.position;
        Vector3 directionToWalkPoint = (walkPoint - transform.position).normalized;
    
        // Ground check - true 
        bool groundHit = Physics.Raycast(walkPoint, Vector3.down, out RaycastHit groundHitInfo, 5f, groundLayer);
        
        // Obstacle check - false
        //bool obstacleHit = Physics.Raycast(walkPoint + new Vector3(0f, 1f, 0f), Vector3.up, out RaycastHit obstacleHitInfo, 5f, obstacleLayer);
        //transform.LookAt(walkPoint);

        Debug.DrawRay(transform.position, directionToWalkPoint * distanceToWalkPoint.magnitude, Color.yellow, 5f);
        
        // Obstacle check through the path - false
        // bool obstacleHitThrough = Physics.Raycast(transform.position, directionToWalkPoint, out RaycastHit obstacleTroughHitInfo, distanceToWalkPoint.magnitude + 1f, obstacleLayer);
        
        if (groundHit) // && !obstacleHit && !obstacleHitThrough
        {
            walkPointSet = true;
            return;
        }

        walkPointSet = false;
    }

    private void Attack()
    {
        navAgent.SetDestination(transform.position);
        transform.LookAt(playerObject.transform);

        if (!hasAttacked) // hasAttacked false
        {
            Rigidbody projectileRigidB = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            projectileRigidB.AddForce(transform.forward  * 32f, ForceMode.Impulse);
            projectileRigidB.AddForce(transform.up * 8f, ForceMode.Impulse);
            
            hasAttacked = true;
            Invoke(nameof(ResetAttack), atttackCooldown);
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }

    private void Follow()
    {
        navAgent.SetDestination(playerObject.position);
    }

    private void TakDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Invoke(nameof(DestroyEnemy), .5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
