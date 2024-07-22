using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInputText : MonoBehaviour
{
    public TextMeshProUGUI UItext;
    public TMP_InputField input;

    void Awake()
    {
        if (PlayerPrefs.HasKey("userName"))
        {
            UItext.text = PlayerPrefs.GetString("userName");
        }
        else
        {
            UItext.text = "Unknown";
        }
    }

    public void Create()
    {
        UItext.text = input.text;
        PlayerPrefs.SetString("userName", input.text);
        PlayerPrefs.Save();
    }
}