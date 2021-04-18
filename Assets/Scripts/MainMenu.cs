using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Canvas settingsMenu;
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas levelsMenu;
    [SerializeField] private Button lvl1, lvl2, lvl3;
    [SerializeField] private GameObject selectedButtonMainMenu, selectedButtonSettings, selectedButtonLevels;
    [SerializeField] private AudioListener listener;

    private bool eng = false;
    [SerializeField] private Text levelsButton, howToPlayButton, settingsButton, quitButton;
    [SerializeField] private Text soundButton, languageButton, backToMenuButton1;
    [SerializeField] private Text level1button, level2button, level3button, backToMenuButton2;

    public void Awake()
    {
        if (PlayerPrefs.GetString("volume") == "off") AudioListener.volume = 0f; else AudioListener.volume = 1f;
        SetLanguage();
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
        mainMenu.gameObject.SetActive(false);
        levelsMenu.gameObject.SetActive(true);
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
        levelsMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedButtonMainMenu);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Language()
    {
        eng = !eng;
        if (eng) PlayerPrefs.SetString("lang", "eng"); else PlayerPrefs.SetString("lang", "ru");
        SetLanguage();
    }

    private void SetLanguage()
    {
        switch (PlayerPrefs.GetString("lang"))
        {
            case "eng":
                levelsButton.text = "LEVELS";
                howToPlayButton.text = "HOW TO PLAY";
                settingsButton.text = "SETTINGS";
                quitButton.text = "QUIT";

                if (PlayerPrefs.GetString("volume") == "on") soundButton.text = "SOUND: ON"; else soundButton.text = "SOUND: OFF";
                languageButton.text = "LANGUAGE: ENG";
                backToMenuButton1.text = "BACK TO MENU";

                level1button.text = "LEVEL 1";
                level2button.text = "LEVEL 2";
                level3button.text = "LEVEL 3";
                backToMenuButton2.text = "BACK TO MENU";
                break;
            case "ru":
                levelsButton.text = "ÓĞÎÂÍÈ";
                howToPlayButton.text = "ÎÁÓ×ÅÍÈÅ";
                settingsButton.text = "ÍÀÑÒĞÎÉÊÈ";
                quitButton.text = "ÂÛÕÎÄ";

                if (PlayerPrefs.GetString("volume") == "on") soundButton.text = "ÇÂÓÊ: ÂÊË"; else soundButton.text = "ÇÂÓÊ: ÂÛÊË";
                languageButton.text = "ßÇÛÊ: ĞÓÑ";
                backToMenuButton1.text = "ÍÀÇÀÄ Â ÌÅÍŞ";

                level1button.text = "ÓĞÎÂÅÍÜ 1";
                level2button.text = "ÓĞÎÂÅÍÜ 2";
                level3button.text = "ÓĞÎÂÅÍÜ 3";
                backToMenuButton2.text = "ÍÀÇÀÄ Â ÌÅÍŞ";
                break;
        }

        
    }
    public void SoundSwitch()
    {
        if (soundButton.text == "ÇÂÓÊ: ÂÊË" || soundButton.text == "SOUND: ON")
        {
            //âûêëş÷åíèå çâóêà
            AudioListener.volume = 0f;
            PlayerPrefs.SetString("volume", "off");
            switch (PlayerPrefs.GetString("lang"))
            {
                case "eng":
                    soundButton.text = "SOUND: OFF";
                    break;
                case "ru":
                    soundButton.text = "ÇÂÓÊ: ÂÛÊË";
                    break;
            }
        }
        else
        {
            //âêëş÷åíèå çâóêà
            AudioListener.volume = 1f;
            PlayerPrefs.SetString("volume", "on");
            switch (PlayerPrefs.GetString("lang"))
            {
                case "eng":
                    soundButton.text = "SOUND: ON";
                    break;
                case "ru":
                    soundButton.text = "ÇÂÓÊ: ÂÊË";
                    break;
            }
        }
    }
}
