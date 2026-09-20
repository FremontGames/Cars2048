using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Commons;
using Commons.UI;

class SettingsDialog : MonoBehaviour
{
    public GameObject LanguageDialog;
    public GameObject HelpDialog;
    public GameObject ThemeDialog;
    public AudioSource SoundAudioSource;

    GameObject SoundButtonOn;
    GameObject SoundButtonOff;

    void Start()
    {
//        gameObject.FindChild("OkButton").OnClick(CloseAction);
        gameObject.FindChild("CloseButton").OnClick(CloseAction);
        gameObject.FindChild("LanguageButton").OnClick(LanguageOpenAction);
        gameObject.FindChild("HelpButton").OnClick(HelpOpenAction);
//        gameObject.FindChild("ThemeButton").OnClick(ThemeOpenAction);

        GameObject SoundButton = gameObject.FindChild("SoundButton");
        SoundButtonOn = SoundButton.FindChild("On", true);
        SoundButtonOff = SoundButton.FindChild("Off", true);
        SoundButton.OnClick(ToggleSoundAction);

        UpdateUI();
    }

    void OnEnable()
    {
        Main.Instance.Theme.Apply(gameObject);
        Main.Instance.Lang.Apply(gameObject, true);
    }

    public void UpdateUI()
    {
        bool sound = PlayerPrefsHelper.GetBool(PlayerPrefsHelper.SOUND);
        SoundButtonOn.SetActive(sound);
        SoundButtonOff.SetActive(!sound);
    }

    internal void CloseAction()
    {
        gameObject.SetActive(false);
    }

    internal void LanguageOpenAction()
    {
        gameObject.SetActive(false);
        LanguageDialog.SetActive(true);
    }

    internal void HelpOpenAction()
    {
        gameObject.SetActive(false);
        HelpDialog.SetActive(true);
    }

    internal void ThemeOpenAction()
    {
        gameObject.SetActive(false);
        ThemeDialog.SetActive(true);
    }

    internal void ToggleSoundAction()
    {
        bool state = PlayerPrefsHelper.GetBool(PlayerPrefsHelper.SOUND);
        PlayerPrefsHelper.SetBool(PlayerPrefsHelper.SOUND, !state);
        if (SoundAudioSource)
            SoundAudioSource.mute = state;
        UpdateUI();
    }

}


