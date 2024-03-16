using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject[] selectors;
    void Start()
    {
        SetNewLocale(PlayerPrefs.GetString("lang", "en"));
    }

    public void SetNewLocale(string language)
    {
        switch (language)
        {
            case "en":
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                selectors[0].SetActive(true);
                selectors[1].SetActive(false);
                break;
            case "pt":
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                selectors[0].SetActive(false);
                selectors[1].SetActive(true);
                break;
            default:
                break;
        }
        PlayerPrefs.SetString("lang", language);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
