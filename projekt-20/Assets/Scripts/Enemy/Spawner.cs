using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] public float respawnTime = 3f;
    public Transform[] spawnPoints;

    private Transform _current;
    private Vector3 _currentPos;
    private Quaternion _currentRot;

    void Start()
    {
        SpawnAll();
    }

    void SpawnAll()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnEnemy(spawnPoint);
        }
    }

    private void SpawnEnemy(Transform spawnPoint)
    {
        _current = spawnPoint;
        _currentPos = _current.position;
        _currentRot = _current.rotation;


        GameObject currentEnemyObject = Instantiate(enemyPrefab, _currentPos, _currentRot);
        EnemyAI enemyAI = currentEnemyObject.GetComponent<EnemyAI>();
        enemyAI.origin = spawnPoint;
        

        enemyAI.OnDeath += HandleDeath;
    }

    private void HandleDeath(EnemyAI enemyAI)
    {
        enemyAI.OnDeath -= HandleDeath;
        StartCoroutine(DelayRespawn(enemyAI.origin));
    }

    private System.Collections.IEnumerator DelayRespawn(Transform origin)
    {
        yield return new WaitForSeconds(respawnTime);
        SpawnEnemy(GetRandomSpawnPoint());
    }

    private Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }
}
