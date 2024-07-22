using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject HighScoreTemplate;

    private List<HighScore> scores;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Time.timeScale = 1f;
        scores = XMLManager.instance.LoadScores();
        scores.Sort((x, y) => y.score.CompareTo(x.score));
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu 1");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu 1")
        {
            LoadHighScores();
        }
        Time.timeScale = 1f;

    }

    public void OpenTutorialScene()
    {
        SceneManager.LoadScene("Tutorial Level");
    }

    private void LoadHighScores()
    {
        //GameObject highScoreSlide = GameObject.FindWithTag("HighScore");

        //int levelCount = PlayerPrefs.HasKey("levelCount") ? PlayerPrefs.GetInt("levelCount") : 0;

        //for (int i = 0; i < levelCount; i++)
        //{
        //    GameObject highScoreObject = Instantiate(HighScoreTemplate, highScoreSlide.transform);

        //    Vector3 currentPosition = highScoreObject.transform.localPosition;
        //    currentPosition.y -= i*50;
        //    highScoreObject.transform.localPosition = currentPosition;

        //    string indexString = "level_" + i;
        //    string levelName = PlayerPrefs.GetString(indexString);

        //    Transform childTransform = highScoreObject.transform.Find("Level Name");
        //    if (childTransform != null)
        //    {
        //        GameObject levelNameUI = childTransform.gameObject;
        //        TextMeshProUGUI levelNameTMPro = levelNameUI.GetComponent<TextMeshProUGUI>();
        //        levelNameTMPro.text = levelName;
        //    }

        //    int score = PlayerPrefs.GetInt(levelName);
        //    childTransform = highScoreObject.transform.Find("High Score");
        //    if (childTransform != null)
        //    {
        //        GameObject highScoreUI = childTransform.gameObject;
        //        TextMeshProUGUI highScoreTMPro = highScoreUI.GetComponent<TextMeshProUGUI>();
        //        highScoreTMPro.text = score.ToString();
        //    }
        //}
        if (scores == null)
        {
            scores = XMLManager.instance.LoadScores();
            scores.Sort((x, y) => y.score.CompareTo(x.score));
        }

        GameObject highScoreSlide = GameObject.FindWithTag("HighScore");

        for (int i = 0; i < scores.Count; i++)
        {
            GameObject highScoreObject = Instantiate(HighScoreTemplate, highScoreSlide.transform);

            Vector3 currentPosition = highScoreObject.transform.localPosition;
            currentPosition.y -= i * 50;
            highScoreObject.transform.localPosition = currentPosition;

            Transform childTransform = highScoreObject.transform.Find("Level Name");
            if (childTransform != null)
            {
                TextMeshProUGUI levelNameTMPro = childTransform.gameObject.GetComponent<TextMeshProUGUI>();
                levelNameTMPro.text = scores[i].playerName + " - " + scores[i].levelName;
            }

            childTransform = highScoreObject.transform.Find("High Score");
            if (childTransform != null)
            {
                TextMeshProUGUI highScoreTMPro = childTransform.gameObject.GetComponent<TextMeshProUGUI>();
                highScoreTMPro.text = scores[i].score.ToString();
            }
        }

    }

    public void SaveHighScore(string levelName,int score)
    {
        //Debug.Log("Name " + name +" and " + score);
        //// Saving Level Name if it not exists
        //if (!PlayerPrefs.HasKey(name))
        //{
        //    int levelCount = PlayerPrefs.HasKey("levelCount") ? PlayerPrefs.GetInt("levelCount") : 0;
        //    string indexName = "level_" + levelCount;
        //    PlayerPrefs.SetString(indexName, name);
        //    levelCount++;
        //    PlayerPrefs.SetInt("levelCount", levelCount);
        //}
        //// Checking High Scores and saving it
        //int savedScore = PlayerPrefs.HasKey(name) ? PlayerPrefs.GetInt(name) : 0;
        //savedScore = Mathf.Max(savedScore, score);
        //PlayerPrefs.SetInt(name, savedScore);
        //PlayerPrefs.Save();

        HighScore highScore = new HighScore();
        highScore.playerName = (PlayerPrefs.HasKey("userName") ? PlayerPrefs.GetString("userName") : "Unknown");
        highScore.levelName = levelName;
        highScore.score = score;

        scores.Add(highScore);
        XMLManager.instance.SaveScores(scores);
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}