using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    // Serializable or public Variables
    [SerializeField] private Image ProgressFillImage;
    [SerializeField] private TextMeshProUGUI totalTime;
    [SerializeField] private TextMeshProUGUI elapsedTime;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject completionMenu;
    [SerializeField] private string nextLevel;
    [SerializeField] private TextMeshProUGUI nextLevelTimer;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private XRInteractorLineVisual leftLineVisual; 
    [SerializeField] private XRInteractorLineVisual rightLineVisual;
    
    public List<AudioSource> music;
    public bool isLooping;
    [HideInInspector] public int musicIndex = 0;
    
    // Private internal variables
    private bool isPaused = false;
    private int score;
    private int slicedBoxes = 0;
    private int missedBoxes = 0;
    private bool levelCompleted = false;
    // Start is called before the first frame update
    void Start()
    {
        musicIndex = 0;
        if (music[musicIndex] == null)
        {
            Debug.LogError("Audio Source is not assigned.");
            return;
        }
        score = 0;
        scoreText.text = "Score: " + score;
        totalTime.text = (int)music[musicIndex].clip.length / 60 + ":" + (int)music[musicIndex].clip.length % 60;
    }

    void pauseGame()
    {
        if (music[musicIndex].isPlaying)
        {
            music[musicIndex].Pause();
        }
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenu.SetActive(true);
        leftLineVisual.enabled = true;
        rightLineVisual.enabled = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu 1");
    }
    public void resumeGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            if (!music[musicIndex].isPlaying)
            {
                music[musicIndex].UnPause();
            }
            isPaused = false;
            pauseMenu.SetActive(false);
            leftLineVisual.enabled = false;
            rightLineVisual.enabled = false;
        }
    }

    public void IncreaseScore()
    {
        slicedBoxes++;
        score = score + 1;
        scoreText.text = "Score: " + score;
        float accuracy = ((float)slicedBoxes / (slicedBoxes + missedBoxes)) * 100;
        accuracyText.text = "Accuracy: " + accuracy.ToString("F2") + "%";
    }

    public void missedBox()
    {
        missedBoxes++;
        float accuracy = ((float)slicedBoxes / (slicedBoxes + missedBoxes)) * 100;
        accuracyText.text = "Accuracy: " + accuracy.ToString("F2") + "%";
    }

    public float getAccuracy() { return (float)slicedBoxes / (slicedBoxes + missedBoxes); }

    public void setSpeedText(float speed)
    {
        speedText.text = "Speed:    " + speed.ToString("F1") + "x";
    }

    public void DecreaseScore()
    {
        score = (score > 0)? score - 1 : 0;
        scoreText.text = "Score: " + score;
    }

    void UpdateProgressBar()
    {
        float value = music[musicIndex].time / music[musicIndex].clip.length;
        ProgressFillImage.fillAmount = value;
    }

    public void SaveAndExit()
    {
        GameManager.instance.SaveHighScore((music.Count == 1) ? music[musicIndex].name : "Looping Level", score);
        GameManager.instance.LoadMainMenu();
    }

    private IEnumerator NextLevelAuto()
    {
        int countdown = 7;

        while (countdown > 0)
        {
            if (nextLevelTimer != null)
            {
                nextLevelTimer.text = countdown.ToString() + "s";
            }
            yield return new WaitForSeconds(1);
            countdown--;
        }

        SceneManager.LoadScene(nextLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPaused && !levelCompleted)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                pauseGame();
            }
            UpdateProgressBar();
        }
        if (!levelCompleted && !music[musicIndex].isPlaying && music[musicIndex].time >= music[musicIndex].clip.length && (music.Count == 1 || (musicIndex == music.Count - 1 && !isLooping)))
        {
            levelCompleted = true;
            completionMenu.SetActive(true);
            rightLineVisual.enabled = true;
            leftLineVisual.enabled = true;
            StartCoroutine(NextLevelAuto());
            GameManager.instance.SaveHighScore((music.Count == 1) ? music[musicIndex].name : "Multi-Song Level",score);
        }
        // For playing next music in looping level
        if (!levelCompleted && music[musicIndex].time >= music[musicIndex].clip.length && (musicIndex < music.Count - 1 || isLooping))
        {
            music[musicIndex].Stop();
            musicIndex = (musicIndex + 1) % music.Count;
            music[musicIndex].Play();
        }
        if(!levelCompleted)
        {
            elapsedTime.text = (int)music[musicIndex].time / 60 + ":" + (int)music[musicIndex].time % 60;

        }
    }
}
