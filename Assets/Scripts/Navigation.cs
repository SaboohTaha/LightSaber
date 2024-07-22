using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Navigation : MonoBehaviour
{
    public void OpenTutorialScene()
    {
        SceneManager.LoadScene("Tutorial Level");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu 1");
    }
}
