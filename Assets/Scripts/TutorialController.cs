using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialController : MonoBehaviour
{
    public float spawnEarlyTime;
    public Transform cameraTransform;
    public float SpawnDistance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip welcomeClip;
    [SerializeField] private AudioClip blueRightBoxClip;
    [SerializeField] private AudioClip purpleLeftBoxClip;
    [SerializeField] private AudioClip fourDirectionClip;
    [SerializeField] private AudioClip completionClip;

    [SerializeField] private GameObject blueBoxPrefab;
    [SerializeField] private GameObject purpleBoxPrefab;

    [SerializeField] private GameObject completionMenu;
    [SerializeField] private XRInteractorLineVisual leftLineVisual;
    [SerializeField] private XRInteractorLineVisual rightLineVisual;

    [HideInInspector] public int score = 0;
    private float boxSize = 0.2f;


    void Start()
    {
        StartCoroutine(StartTutorial());
    }

    private IEnumerator StartTutorial()
    {
        // Play welcome clip
        audioSource.clip = welcomeClip;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        bool complete = false;
        // Play Right hand weapon clip
        do
        {
            audioSource.clip = blueRightBoxClip;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            // Spawn Right hand weapon box
            SpawnBlueRightBox();
            yield return new WaitForSeconds(spawnEarlyTime + 1f);
            complete = (score == 1);
        } while (!complete);
        score = 0;

        // Play Left hand weapon clip
        do
        {
            audioSource.clip = purpleLeftBoxClip;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            // Spawn Left hand weapon box
            SpawnPurpleLeftBox();
            yield return new WaitForSeconds(spawnEarlyTime + 1f);
            complete = (score == 1);
        } while (!complete);
        score = 0;

        // Play 4 Direction Clip
        do
        {
            audioSource.clip = fourDirectionClip;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            StartCoroutine(SpawnDirectionalBoxes());
            yield return new WaitForSeconds(spawnEarlyTime + 4f);
            complete = (score == 4);
        } while (!complete);
        score = 0;

        audioSource.clip = completionClip;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        ShowCompletionMenu();
    }

    private void ShowCompletionMenu()
    {
        completionMenu.SetActive(true);
        rightLineVisual.enabled = true;
        leftLineVisual.enabled = true;
    }

    private void SpawnBlueRightBox()
    {
        Vector3 spawnPosition = getSpawnPosition(6);
        Quaternion spawnRotation = getRotation("D");
        GameObject instantiatedObject = Instantiate(blueBoxPrefab, spawnPosition, spawnRotation);

        MoveTowardCamera moveScript = instantiatedObject.GetComponent<MoveTowardCamera>();
        moveScript.InitializeMove(cameraTransform.position, spawnEarlyTime);
    }

    private void SpawnPurpleLeftBox()
    {
        Vector3 spawnPosition = getSpawnPosition(5);
        Quaternion spawnRotation = getRotation("D");
        GameObject instantiatedObject = Instantiate(purpleBoxPrefab, spawnPosition, spawnRotation);

        MoveTowardCamera moveScript = instantiatedObject.GetComponent<MoveTowardCamera>();
        moveScript.InitializeMove(cameraTransform.position, spawnEarlyTime);
    }

    private IEnumerator SpawnDirectionalBoxes()
    {
        // First box
        Vector3 spawnPosition = getSpawnPosition(3);
        Quaternion spawnRotation = getRotation("U");
        GameObject instantiatedObject = Instantiate(blueBoxPrefab, spawnPosition, spawnRotation);

        MoveTowardCamera moveScript = instantiatedObject.GetComponent<MoveTowardCamera>();
        moveScript.InitializeMove(cameraTransform.position, spawnEarlyTime);
        yield return new WaitForSeconds(1);

        // Second Box
        spawnPosition = getSpawnPosition(4);
        spawnRotation = getRotation("L");
        instantiatedObject = Instantiate(purpleBoxPrefab, spawnPosition, spawnRotation);

        moveScript = instantiatedObject.GetComponent<MoveTowardCamera>();
        moveScript.InitializeMove(cameraTransform.position, spawnEarlyTime);
        yield return new WaitForSeconds(1);

        // Third Box
        spawnPosition = getSpawnPosition(7);
        spawnRotation = getRotation("R");
        instantiatedObject = Instantiate(blueBoxPrefab, spawnPosition, spawnRotation);

        moveScript = instantiatedObject.GetComponent<MoveTowardCamera>();
        moveScript.InitializeMove(cameraTransform.position, spawnEarlyTime);
        yield return new WaitForSeconds(1);

        // Fourth Box
        spawnPosition = getSpawnPosition(13);
        spawnRotation = getRotation("D");
        instantiatedObject = Instantiate(purpleBoxPrefab, spawnPosition, spawnRotation);

        moveScript = instantiatedObject.GetComponent<MoveTowardCamera>();
        moveScript.InitializeMove(cameraTransform.position, spawnEarlyTime);
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
            Debug.LogError("Wrong rotation input: " + rotation);
            return Quaternion.Euler(0f, 0f, 0f);
        }
    }

    Vector3 getSpawnPosition(int position)
    {
        float x, y, z;
        x = cameraTransform.position.x - SpawnDistance;
        y = cameraTransform.position.y - (position / 4 - 2 + (position / 4 >= 2 ? 1 : 0)) * boxSize;
        z = cameraTransform.position.z + (position % 4 - 2) * boxSize;
        return new Vector3(x, y, z);
    }

}
