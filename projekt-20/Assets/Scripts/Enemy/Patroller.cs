using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Patroller : MonoBehaviour
{
    [SerializeField] private int patrolRadius = 10;
    [SerializeField] private Vector3 patrolPoint;
    
    
    [Header("NavMesh Components")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private NavMeshSurface navSurface;

    private bool pointSet;
    
    
    void Update()
    {
        // If no patrol point, generate one
        if (!pointSet) 
            patrolPoint = GeneratePatrolPoint();
        
        // Set destination
        if (pointSet)
        {
            Debug.Log("Point has been set to: " + patrolPoint);
            navAgent.SetDestination(patrolPoint);
        }

        Vector3 currentDistance = transform.position - patrolPoint;

        if (currentDistance.magnitude < .1f)
            pointSet = false;
    }
    
    private Vector3 GeneratePatrolPoint()
    {
        int randomX = Random.Range(-patrolRadius, patrolRadius);
        int randomZ = Random.Range(-patrolRadius, patrolRadius);

        patrolPoint = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z + randomZ);
        
        pointSet = true;
        
        return patrolPoint;
    }
    
}
