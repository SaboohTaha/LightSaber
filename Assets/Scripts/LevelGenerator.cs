using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct SpawnEvent
{
    public float timestamp;
    public int gameObjectIndex; 
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
}


public class LevelGenerator : MonoBehaviour
{
    private LevelController levelController;


    public float spawnEarlyTime;
    public Transform cameraTransform;
    public float SpawnDistance;
    [SerializeField] private List<TextAsset> spawnEventsFile;
    [SerializeField] public GameObject BlueBox1;
    [SerializeField] public GameObject PurpleBox2;
    [SerializeField] private float boxSize = 0.2f;


    private float speed = 1f;
    private float nextLevelCheckTime = 30f;
    private List<List<SpawnEvent>> spawnEvents = new List<List<SpawnEvent>>();
    private int currentIndex = 0;
    private int musicIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        musicIndex = 0;
        levelController = GetComponent<LevelController>();
        if (levelController.music[musicIndex] == null)
        {
            Debug.LogError("Audio Source is not assigned.");
            return;
        }
        LoadSpawnEvents();

        levelController.music[musicIndex].Play();
    }
    void LoadSpawnEvents()
    {
        if (spawnEventsFile.Count != 0)
        {
            for (int i = 0; i < spawnEventsFile.Count; i++)
            {
                List<SpawnEvent> singleSpawnEvent = new List<SpawnEvent>();
                string[] lines = spawnEventsFile[i].text.Split('\n');

                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    string[] values = trimmedLine.Split(',');

                    if (values.Length == 4)
                    {
                        SpawnEvent spawnEvent = new SpawnEvent();
                        spawnEvent.timestamp = float.Parse(values[0]);
                        spawnEvent.gameObjectIndex = int.Parse(values[1]);
                        spawnEvent.spawnPosition = getSpawnPosition(int.Parse(values[2]));
                        spawnEvent.spawnRotation = getRotation(values[3]);
                        singleSpawnEvent.Add(spawnEvent);
                    }
                    else
                    {
                        Debug.LogWarning("Invalid line in the text file: " + line);
                    }
                }
                spawnEvents.Add(singleSpawnEvent);
            }
        }
        else
        {
            Debug.LogError("Spawn events file is not assigned.");
        }
    }

    Quaternion getRotation(string rotation)
    {
        rotation = rotation.ToLower();
        if (rotation == "u")
        {
            return Quaternion.Euler(180f, 0f, 0f);
        }
        else if (rotation == "d")
        {
            return Quaternion.Euler(0f, 0f, 0f);
        }
        else if (rotation == "l")
        {
            return Quaternion.Euler(90f, 0f, 0f);
        }
        else if (rotation == "r")
        {
            return Quaternion.Euler(-90f, 0f, 0f);
        }
        else
        {
            Debug.LogError("Wrong rotation input: "+rotation);
            return Quaternion.Euler(0f, 0f, 0f);
        }
    }

    Vector3 getSpawnPosition(int position)
    {
        float x, y, z;
        x = cameraTransform.position.x - SpawnDistance;
        y = cameraTransform.position.y - (position / 4 - 2 + (position/4>=2 ? 1:0)) * boxSize;
        z = cameraTransform.position.z + (position % 4 - 2) * boxSize;
        return new Vector3(x, y, z);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentIndex < spawnEvents[musicIndex].Count)
        {
            if (levelController.music[musicIndex].time >= spawnEvents[musicIndex][currentIndex].timestamp - spawnEarlyTime)
            {
                GameObject prefabToInstantiate = spawnEvents[musicIndex][currentIndex].gameObjectIndex == 1 ? BlueBox1 : PurpleBox2;
                GameObject instantiatedObject = Instantiate(prefabToInstantiate, spawnEvents[musicIndex][currentIndex].spawnPosition, spawnEvents[musicIndex][currentIndex].spawnRotation);

                MoveTowardCamera moveScript = instantiatedObject.GetComponent<MoveTowardCamera>();
                moveScript.InitializeMove(cameraTransform.position, spawnEarlyTime);

                currentIndex++;
            }
            if (levelController.music[musicIndex].time > nextLevelCheckTime)
            {
                float accuracy = levelController.getAccuracy();
                if (accuracy > 0.7)
                {
                    if (speed < 2.0f)
                    {
                        speed += 0.2f;
                        spawnEarlyTime -= 0.3f;
                    }
                }
                else if (accuracy < 0.5)
                {
                    if (speed > 0.4f)
                    {
                        speed -= 0.2f;
                        spawnEarlyTime += 0.3f;
                    }
                }
                levelController.setSpeedText(speed);
                nextLevelCheckTime += 30f;
            }
        }
        if (musicIndex != levelController.musicIndex)
        {
            musicIndex = levelController.musicIndex;
            currentIndex = 0;
        }
    }
}
