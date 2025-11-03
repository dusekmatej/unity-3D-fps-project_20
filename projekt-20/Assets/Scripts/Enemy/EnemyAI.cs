using System;
using System.Threading;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [Header ("NavMesh Components")]
    public NavMeshAgent navAgent;
    public Transform playerObject;
    public LayerMask groundLayer, playerLayer, obstacleLayer;
    
    private Vector3 _patrolPoint;
    private bool _pointSet;
    public float pointThreshDistance;
    public GameObject projectile;
    public float health;

    public float atttackCooldown = 2f;
    private bool hasAttacked;

    public float detectRange, attackRange;

    private bool inRange;
    private bool inSight;

    private bool lastInSight;
    private bool lastInRange;
    
    
    // TODO: Patrol mechanic
    // TODO: Player chase mechanic
    // TODO: Attack mechanic - not laggy one
    // TODO: Health system
    // TODO: Spawn system
    
    void Awake()
    {
        playerObject = GameObject.FindWithTag("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();

        if (pointThreshDistance <= 0f) pointThreshDistance = 8f;

        if (navAgent != null)
        {
            navAgent.updatePosition = true;
            navAgent.updateRotation = true;
            navAgent.isStopped = false;
        }

        if (TryGetComponent<Rigidbody>(out var rb) && !rb.isKinematic)
        {
            Debug.Log("EnemyAI Awake: Found non kinematic rigidbody");
        }
    }

    private void Update()
    {
        // Safety check
        if (navAgent == null || playerObject == null) return;

        
        float distanceToPlayer = Vector3.Distance(transform.position, playerObject.position);

        inRange = distanceToPlayer <= attackRange;
        inSight = distanceToPlayer <= detectRange && PlayerVisible();

        bool hasSight = false;
        if (inSight && inRange)
        {
            Vector3 directionToPlayer = (playerObject.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, detectRange))
                hasSight = PlayerVisible();
        }


        inSight = distanceToPlayer <= detectRange;
        inRange = distanceToPlayer <= attackRange;
        
        if (!inSight && !inRange) Patrol(); 
        
        
        if (inSight && !inRange) Follow();
        if (inRange && inSight) Attack();
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
    
    // if no patrol point, generate one and then set it as destination
    private void Patrol()
    {
        if (navAgent == null)
        {
            Debug.Log("Patrol: navAgent is null");
        }
        
        // Quick agent health checks
        if (!navAgent.enabled)
            Debug.Log("Patrol: navAgent.disabled = true");
        if (!navAgent.isOnNavMesh)
        {
            Debug.Log("Patrol: navAgent is not on the NavMesh");
            return;
        }
        
        Debug.Log($"Patrol start: _pointSet={_pointSet} speed={navAgent.speed} isStopped={navAgent.isStopped} hasPath={navAgent.hasPath} remaining={navAgent.remainingDistance} pathStatus={navAgent.pathStatus}");

        if (!_pointSet)
        {
            _patrolPoint = GeneratePointLocation();

            if (_pointSet)
            {
                navAgent.SetDestination(_patrolPoint);
                navAgent.isStopped = false;
            }
        }
        else
        {
            if (!navAgent.hasPath && !navAgent.pathPending)
            {
                navAgent.SetDestination(_patrolPoint);
            }
        }

        float arrivalThreshold = 0.4f;
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(_patrolPoint.x, 0, _patrolPoint.z)) < arrivalThreshold)
        {
            _pointSet = false;
            Debug.Log("Patrol: reached point, clearing _pointSet");
        }
        
        if (!_pointSet)
        {
            Debug.Log("Patrol: no patrol point found");
            return;
        }
        
        Vector3 pointDistance = transform.position - _patrolPoint;

        if (pointDistance.magnitude < 0.5f)
        {
            _pointSet = false;
            Debug.Log("Patrol: reached point, clearing _pointSet");
        }
    }

    // Generation of patrol point within threshold distance
    private Vector3 GeneratePointLocation()
    {
        int maxAttempts = 30;
        float sampleDistance = Mathf.Max(2f, pointThreshDistance);
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Debug.Log("Generating patrol point, attempt: " + attempt);
            Vector3 randomOffset = Random.insideUnitSphere * pointThreshDistance;
            randomOffset.y = 0f;
            Vector3 _patrolPoint = transform.position + randomOffset;
            
            if (NavMesh.SamplePosition(_patrolPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                Debug.Log($"  Sampled navmesh pos: {hit.position}");

                if (Vector3.Distance(hit.position, transform.position) < 0.5f)
                {
                    Debug.Log("  Hit too close to current position, skipping.");
                    continue;
                }
                
                NavMeshPath path = new NavMeshPath();
                if (navAgent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    _patrolPoint = hit.position;
                    _pointSet = true;
                    Debug.Log($"Selected patrol point: {_patrolPoint}");
                    return _patrolPoint;
                }
                else
                {
                    Debug.Log($"Path incomplete or invalid");
                }
            }
        }
        
        if (NavMesh.SamplePosition(transform.position + transform.forward * 1f, out NavMeshHit fallbackHit, sampleDistance, NavMesh.AllAreas))
        {
            _patrolPoint = fallbackHit.position;
            _pointSet = true;
            return _patrolPoint;
        }
        
        _pointSet = false;
        return transform.position;
    }

    private void Attack()
    {
        navAgent.SetDestination(transform.position);
        transform.LookAt(playerObject.transform);

        if (!hasAttacked) // hasAttacked false
        {
            Rigidbody projectileRigidB = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            projectileRigidB.AddForce(transform.forward  * 32f, ForceMode.Impulse);
            projectileRigidB.AddForce(transform.up * 4f, ForceMode.Impulse);
            
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
