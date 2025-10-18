/*using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            
            navMeshAgent.SetDestination(player.position);
        }
    }
}*/