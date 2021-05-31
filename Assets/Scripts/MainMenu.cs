using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Canvas settingsMenu;
    [SerializeField] private Canvas mainMenu;
    [SerializeField] private Canvas levelsMenu;
    [SerializeField] private Button lvl1, lvl2, lvl3;
    [SerializeField] private GameObject selectedButtonMainMenu, selectedButtonSettings, selectedButtonLevels, selectedButtonStats;
    [SerializeField] private Text vol;

    private LocalizedString volume = new LocalizedString { TableReference = "UI"};

    void UpdateString(string translatedValue)
    {
        vol.text = translatedValue;
    }

    public void Awake()
    {
        Statistics.instance.Save();
        Statistics.instance.Upd();
        Statistics.instance.go.SetActive(false);

        volume.StringChanged += UpdateString;
        if (PlayerPrefs.GetString("volume") == "off")
        {
            AudioListener.volume = 0f;
            volume.TableEntryReference = "vol off";
        }
        else
        {
            AudioListener.volume = 1f;
            volume.TableEntryReference = "vol on";
        }
        volume.RefreshString();

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

    public void Stats()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedButtonStats);
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
        volume.RefreshString();
    }

    public void SoundSwitch()
    {
        if (AudioListener.volume == 1f)
        {
            //выключение звука
            volume.TableEntryReference = "vol off";
            volume.GetLocalizedString(volume.Arguments);
            AudioListener.volume = 0f;
            PlayerPrefs.SetString("volume", "off");
        }
        else
        {
            //включение звука
            volume.TableEntryReference = "vol on";
            volume.GetLocalizedString(volume.Arguments);
            AudioListener.volume = 1f;
            PlayerPrefs.SetString("volume", "on");
        }
        volume.RefreshString();
    }
}
