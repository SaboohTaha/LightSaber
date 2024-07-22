using UnityEngine;

public class MusicSync : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public CubeSpawner cubeSpawner; // Reference to the CubeSpawner script or object
    public float spawnDelay = 2.0f; // Add a delay between cube spawns

    private float lastSpawnTime;

    void Update()
    {
        if (audioSource.isPlaying)
        {
            float musicTime = audioSource.time;

            // Adjust spawn timing based on music time
            if (musicTime > lastSpawnTime + spawnDelay)
            {
                cubeSpawner.SpawnCubes(); // Call the method to spawn cubes
                lastSpawnTime = musicTime;
            }
        }
    }
}
