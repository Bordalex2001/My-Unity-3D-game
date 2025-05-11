using UnityEngine;

public class BearSpawnerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private int maxBears = 1;

    private float spawnInterval = 10f;
    private int currentBears = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnBear), 0f, spawnInterval);
    }

    private void SpawnBear()
    { 
        if (currentBears >= maxBears) return;

        GameObject bear = Instantiate(obj, transform.position, Quaternion.identity);
        currentBears++;

        bear.GetComponent<BearAIScript>().OnBearDeath += () => currentBears--;
    }
}