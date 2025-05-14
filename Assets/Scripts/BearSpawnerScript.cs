using UnityEngine;

public class BearSpawnerScript : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private GameObject bearPrefab;
    [SerializeField]
    private int maxBears = 1;

    private float spawnRadius = 10f;
    private float spawnInterval = 5f;
    private int currentCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnBear), 0f, spawnInterval);
    }

    private void SpawnBear()
    { 
        if (currentCount >= maxBears) return;

        Vector3 spawnPosition = GetSafeSpawnPosition();
        GameObject bear = Instantiate(bearPrefab, spawnPosition, Quaternion.identity);

        var bearAI = bear.GetComponent<BearAIScript>();
        bearAI.Init(player, 2.5f, 5f);
        bearAI.OnDeath += () => currentCount--;

        currentCount++;
    }

    private Vector3 GetSafeSpawnPosition() 
    { 
        for (int i = 0; i < 10; i++)
        {
            Vector3 offset = Random.insideUnitSphere * spawnRadius;
            offset.y = 0; // Keep the bear on the same plane
            Vector3 position = transform.position + offset;

            if (Physics.Raycast(position + Vector3.up * 3f, Vector3.down, out RaycastHit hit, 5f))
            {
                return hit.point;
            }
        }

        return transform.position;
    }
}