using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Item key;
    [SerializeField] private Item key2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (sceneName == "FirstLevel")
            {
                SceneManager.LoadScene(sceneName);
                if (PlayerPrefs.GetInt("levels") < 1)
                    PlayerPrefs.SetInt("levels", 1);
            }

            else if (sceneName == "SecondLevel")
            {
                if (InventoryManager.instance.items.Contains(key) && InventoryManager.instance.items.Contains(key2))
                {
                    SceneManager.LoadScene(sceneName);
                    if (PlayerPrefs.GetInt("levels") < 2)
                        PlayerPrefs.SetInt("levels", 2);
                }
            }

            else if (sceneName == "ThirdLevel")
            {
                if (InventoryManager.instance.items.Contains(key2))
                {
                    SceneManager.LoadScene(sceneName);
                    if (PlayerPrefs.GetInt("levels") < 3)
                        PlayerPrefs.SetInt("levels", 3);
                }
            }
        }
    }
}
