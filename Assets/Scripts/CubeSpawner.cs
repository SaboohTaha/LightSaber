using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public Vector3 spawnAreaSize;
    public float spawnRate = 2.0f;

    void Start()
    {
        StartCoroutine(SpawnCubesRoutine());
    }

    IEnumerator SpawnCubesRoutine()
    {
        while (true)
        {
            SpawnCubes();
            yield return new WaitForSeconds(1.0f / spawnRate);
        }
    }

    public void SpawnCubes()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        for (int i = 0; i < 1; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                                                 Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
                                                 Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2));

            GameObject cube = Instantiate(cubePrefab, randomPosition, Quaternion.identity);
            Rigidbody cubeRigidbody = cube.GetComponent<Rigidbody>();

            if (cubeRigidbody != null)
            {
                cubeRigidbody.AddForce((player.transform.position - cube.transform.position).normalized * 5f, ForceMode.Impulse);
            }
        }
    }
}
