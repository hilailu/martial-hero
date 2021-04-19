using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Canvas settingsMenu;
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas levelsMenu;
    [SerializeField] private Button lvl1, lvl2, lvl3;
    [SerializeField] private GameObject selectedButtonMainMenu, selectedButtonSettings, selectedButtonLevels;
    [SerializeField] private AudioListener listener;


    public void Awake()
    {
        if (PlayerPrefs.GetString("volume") == "off") AudioListener.volume = 0f; else AudioListener.volume = 1f;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedButtonMainMenu);
        if (PlayerPrefs.GetInt("levels") >= 1)
        {
            lvl1.interactable = true;
        }
        else lvl1.interactable = false;
        if (PlayerPrefs.GetInt("levels") >= 2)
        {
            lvl2.interactable = true;
        }
        else lvl2.interactable = false;
        if (PlayerPrefs.GetInt("levels") == 3)
        {
            lvl3.interactable = true;
        }
        else lvl3.interactable = false;
    }

    public void Levels()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedButtonLevels);
    }

    public void Level1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("FirstLevel");
        PlayerPrefs.SetInt("paused", 0);
    }
    public void Level2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SecondLevel");
        PlayerPrefs.SetInt("paused", 0);
    }
    public void Level3()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("ThirdLevel");
        PlayerPrefs.SetInt("paused", 0);
    }

    public void HowToPlay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
        PlayerPrefs.SetInt("paused", 0);
    }

    public void Settings()
    {
        mainMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedButtonSettings);
    }

    public void BackToMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedButtonMainMenu);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Language()
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        else LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
    }

    public void SoundSwitch()
    {
        if (AudioListener.volume == 1f)
        {
            //���������� �����
            AudioListener.volume = 0f;
            PlayerPrefs.SetString("volume", "off");
        }
        else
        {
            //��������� �����
            AudioListener.volume = 1f;
            PlayerPrefs.SetString("volume", "on");
        }
    }
}
