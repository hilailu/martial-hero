using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    public int deaths;
    public int allCoins;
    public int enemiesKilled;

    public GameObject go;

    private Text stats;
    private LocalizedString stat = new LocalizedString { TableReference = "UI", Arguments = new List<object>() };

    public static Statistics instance;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
            stat.StringChanged += UpdateString;
            stat.Arguments.Add(this);

            deaths = PlayerPrefs.GetInt("deaths");
            allCoins = PlayerPrefs.GetInt("allCoins");
            enemiesKilled = PlayerPrefs.GetInt("enemiesKilled");
        }
    }

    void UpdateString(string translatedValue)
    {
        stats.text = translatedValue;
    }

    private void OnDestroy()
    {
        stat.StringChanged -= UpdateString;
    }

    public void Upd()
    {
        stats = GameObject.FindWithTag("Stats").GetComponent<Text>();

        stat.TableEntryReference = "statistics";
        stat.GetLocalizedString(stat.Arguments);
        stat.RefreshString();
        go = GameObject.FindWithTag("StatsCanvas");
        /*Canvas canvas = go.GetComponent<Canvas>();
        canvas.enabled = false;*/
        Debug.Log($"{deaths}, {allCoins}, {enemiesKilled}");
    }

    private void OnApplicationQuit()
    {
        Save();
        Debug.Log("App closed");
    }

    public void Save()
    {
        PlayerPrefs.SetInt("deaths", deaths);
        PlayerPrefs.SetInt("allCoins", allCoins);
        PlayerPrefs.SetInt("enemiesKilled", enemiesKilled);
        Debug.Log($"{deaths}, {allCoins}, {enemiesKilled}");
    }
}
