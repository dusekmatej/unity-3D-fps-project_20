using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
// TODO: Health system - in work

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [Header ("NavMesh Components")]
        public NavMeshAgent navAgent;
        public Transform playerObject;
    
        [Header("Enemy Settings")]
        public float detectRange = 35f;
        public float attackRange = 10f;  
        public float attackCooldown = 2f;
        public float enemyHealth = 50f;
        public float pointThreshDistance = 5f;
        public GameObject projectile;
    
        [Header("Debug options")]
        [SerializeField] private bool debugMode;
        [SerializeField] private bool inRange;
        [SerializeField] private bool inSight;
        [SerializeField] private Vector3 patrolPoint;
        [SerializeField] private float arrivalThreshold = 0.4f;
        [SerializeField] private bool isDead;
        [SerializeField] public Transform origin;
        [SerializeField] private int generationMaxAttempts = 10;
        
        private int[] walkAttackDistances = new int[] {15, 20, 25, 30, 35};

        // Other options not necessary for debugging
        private bool _hasAttacked;
        private bool _pointSet;
        public event Action<EnemyAI> OnDeath;
        public static bool DebugMode;
        
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
        }

        private void Update()
        {
            DebugMode = debugMode;

            // Safety check
            if (navAgent == null || playerObject == null) return;

        
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.position);

            inRange = distanceToPlayer <= attackRange;
            inSight = distanceToPlayer <= detectRange && PlayerVisible();

            bool hasSight = false;
            if (inSight && inRange)
            {
                Vector3 directionToPlayer = (playerObject.position - transform.position).normalized;
                if (Physics.Raycast(transform.position, directionToPlayer, detectRange))
                    hasSight = PlayerVisible();
            }


            inSight = distanceToPlayer <= detectRange;
            inRange = distanceToPlayer <= attackRange;
        
            if (!inSight && !inRange) Patrol(); 
            if (inSight && !inRange) Follow();
            if (inRange && inSight) Attack();
            
            foreach (int distance in walkAttackDistances)
            {
                int roundedDistance = Mathf.RoundToInt(distanceToPlayer);
                if (distance == roundedDistance)
                {
                    // Debug.Log($"Distance matched: {roundedDistance}");
                    WalkAttack();
                }
            }
            
            if (isDead)
                DestroyEnemy();
        }

        private void WalkAttack()
        {
            transform.LookAt(playerObject.transform);

            if (!_hasAttacked)
            {
                Rigidbody projectileRigidB = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                projectileRigidB.AddForce(transform.forward  * 32f, ForceMode.Impulse);
                projectileRigidB.AddForce(transform.up * 4f, ForceMode.Impulse);
            
                _hasAttacked = true;
                Invoke(nameof(ResetAttack), attackCooldown);
            }
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

            if (navAgent == null) return;
            if (!navAgent.enabled || !navAgent.isOnNavMesh) return;
            
            if (!_pointSet)
            {
                patrolPoint = GeneratePointLocation();

                if (_pointSet)
                {
                    navAgent.SetDestination(patrolPoint);
                    navAgent.isStopped = false;
                }
            }
            else
            {
                if (!navAgent.hasPath && !navAgent.pathPending)
                {
                    navAgent.SetDestination(patrolPoint);
                }
            }

            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                    new Vector3(patrolPoint.x, 0, patrolPoint.z)) < arrivalThreshold)
            {
                _pointSet = false;
                if (DebugMode) Debug.Log("Patrol: reached point, clearing _pointSet");
            }
        
            if (!_pointSet)
            {
                if (DebugMode) Debug.Log("Patrol: no patrol point found");
                return;
            }
        
            Vector3 pointDistance = transform.position - patrolPoint;

            if (pointDistance.magnitude < 0.5f)
            {
                _pointSet = false;
                if (DebugMode) Debug.Log("Patrol: reached point, clearing _pointSet");
            }
        }

        // Generation of patrol point within threshold distance
        private Vector3 GeneratePointLocation()
        {
            float sampleDistance = Mathf.Max(2f, pointThreshDistance);
            
            for (int attempt = 0; attempt < generationMaxAttempts; attempt++)
            {
                if (DebugMode) Debug.Log("Generating patrol point, attempt: " + attempt);
                Vector3 randomOffset = Random.insideUnitSphere * pointThreshDistance;
                randomOffset.y = 0f;
                Vector3 generatePatrolPoint = transform.position + randomOffset;
            
                if (NavMesh.SamplePosition(generatePatrolPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    if (DebugMode) Debug.Log($"  Sampled navmesh pos: {hit.position}");

                    if (Vector3.Distance(hit.position, transform.position) < 0.5f)
                    {
                        if (DebugMode) Debug.Log("  Hit too close to current position, skipping.");
                        continue;
                    }
                
                    NavMeshPath path = new NavMeshPath();
                    if (navAgent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                    {
                        generatePatrolPoint = hit.position;
                        _pointSet = true;
                        if (DebugMode) Debug.Log($"Selected patrol point: {generatePatrolPoint}");
                        return generatePatrolPoint;
                    }
                    else
                    {
                        if (DebugMode) Debug.Log($"Path incomplete or invalid");
                    }
                }
            }
        
            if (NavMesh.SamplePosition(transform.position + transform.forward * 1f, out NavMeshHit fallbackHit, sampleDistance, NavMesh.AllAreas))
            {
                patrolPoint = fallbackHit.position;
                _pointSet = true;
                return patrolPoint;
            }
        
            _pointSet = false;
            return transform.position;
        }

    
    
        // Method for attacking the Player
        private void Attack()
        {
            navAgent.SetDestination(transform.position);
            transform.LookAt(playerObject.transform);

            if (!_hasAttacked)
            {
                Rigidbody projectileRigidB = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                projectileRigidB.AddForce(transform.forward  * 32f, ForceMode.Impulse);
                projectileRigidB.AddForce(transform.up * 4f, ForceMode.Impulse);
            
                _hasAttacked = true;
                Invoke(nameof(ResetAttack), attackCooldown);
            }
        }

        private void ResetAttack() => _hasAttacked = false;

        private void Follow()
        {
            navAgent.speed = 4f;
            navAgent.SetDestination(playerObject.position);
        }

        public void TakeDamage(float amount)
        {
            enemyHealth -= amount;
            if (enemyHealth <= 0) Invoke(nameof(DestroyEnemy), .5f);
        }

        private void DestroyEnemy()
        {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
