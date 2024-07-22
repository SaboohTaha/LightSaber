using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveTowardCamera : MonoBehaviour
{
    private Vector3 targetPosition;
    private float duration;
    private LevelController levelController;


    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if ("Tutorial Level" != sceneName)
            levelController = GameObject.FindWithTag("LevelController").GetComponent<LevelController>();
    }
    // Set the target position, start time, and duration when the object is instantiated
    public void InitializeMove(Vector3 targetPosition, float duration)
    {
        this.targetPosition = new Vector3(targetPosition.x,transform.position.y,transform.position.z);
        this.duration = duration;

        // Start moving towards the target position
        StartCoroutine(MoveTowardsTarget());
    }

    private System.Collections.IEnumerator MoveTowardsTarget()
    {
        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the interpolation factor between 0 and 1 based on elapsed time
            float t = elapsedTime / duration;

            // Use Lerp to smoothly interpolate between initial and target positions
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            // Increment elapsed time using real time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the object reaches the exact target position
        transform.position = targetPosition;

        // Perform any cleanup or destruction logic here
        if (levelController != null)
            levelController.missedBox();
        Destroy(gameObject);
    }
}
