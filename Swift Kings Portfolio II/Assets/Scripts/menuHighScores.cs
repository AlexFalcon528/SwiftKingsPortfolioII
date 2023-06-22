using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuHighScores : MonoBehaviour
{
    [SerializeField] string scene;
    [SerializeField] TextMeshProUGUI txt;
    // Start is called before the first frame update
    void Start()
    {
        if(scene != null)
        {
            txt.text = $"High score: {PlayerPrefs.GetInt(scene)}";
        }
        
    }
}
