using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string sceneName;
    //private int level;
   // private void Awake()
   // {
    //    level = PlayerPrefs.GetInt("levels");
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(sceneName);
            if (sceneName == "FirstLevel" && PlayerPrefs.GetInt("levels") < 1) PlayerPrefs.SetInt("levels", 1);
            if (sceneName == "SecondLevel" && PlayerPrefs.GetInt("levels") < 2) PlayerPrefs.SetInt("levels", 2);
            if (sceneName == "ThirdLevel" && PlayerPrefs.GetInt("levels") < 3) PlayerPrefs.SetInt("levels", 3);
        }
    }
}
