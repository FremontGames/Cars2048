using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons;
using Commons.Lang;
using Commons.UI;
using SimpleJSON;
using Project2048;

class PlayerPrefsHelper
{
    public const string LANG = "app.settings_lang";
    public const string SOUND = "app.settings_sound";
    public const string MUSIC = "app.settings_music";
    public const string THEME = "app.settings_theme2";
    public const string LEVEL = "save.level_";
    public const string LAST_PLAYED = "save.last_played";
    public const string HELP_AT_STARTUP = "pref.help_at_startup";


    public static bool GetBool(string key)
    {
        if (Strings.IsNullOrEmpty(PlayerPrefs.GetString(key)))
            return false;
        return Boolean.Parse(PlayerPrefs.GetString(key));
    }
    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetString(key, value.ToString());
    }

    public void LoadThenInitializeIfNeeded()
    {
        CheckOrInit(LANG, Application.systemLanguage.ISO() ?? "en");
        CheckOrInit(SOUND, Boolean.TrueString);
        CheckOrInit(MUSIC, Boolean.TrueString);
        CheckOrInit(THEME, Globals.THEME);
        CheckOrInit(HELP_AT_STARTUP, Boolean.TrueString);

        Globals.THEME = PlayerPrefs.GetString(THEME);

        setDefaultSave("0", 2);
        setDefaultSave("101", 8);
    }

    private static void setDefaultSave(string levelId, int coins)
    {
        var saveId = PlayerPrefsHelper.LEVEL + levelId;
        int level_score = PlayerPrefs.GetInt(saveId);
        if (level_score == 0)
            PlayerPrefs.SetInt(saveId, coins);
    }

    private void CheckOrInit(string key, string defaultValue)
    {
        if (Strings.IsNullOrEmpty(PlayerPrefs.GetString(key)))
            PlayerPrefs.SetString(key, defaultValue);
    }
}