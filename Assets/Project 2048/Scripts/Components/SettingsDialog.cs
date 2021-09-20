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
    public AudioSource MusicAudioSource;

    GameObject SoundButtonOn;
    GameObject SoundButtonOff;
    GameObject MusicButtonOn;
    GameObject MusicButtonOff;

    void Start()
    {
        gameObject.FindChild("OkButton").OnClick(CloseAction);
        gameObject.FindChild("CloseButton").OnClick(CloseAction);
        gameObject.FindChild("LanguageButton").OnClick(LanguageOpenAction);
        gameObject.FindChild("HelpButton").OnClick(HelpOpenAction);
        gameObject.FindChild("ThemeButton").OnClick(ThemeOpenAction);

        GameObject SoundButton = gameObject.FindChild("SoundButton");
        SoundButtonOn = SoundButton.FindChild("On", true);
        SoundButtonOff = SoundButton.FindChild("Off", true);
        SoundButton.OnClick(ToggleSoundAction);

        GameObject MusicButton = gameObject.FindChild("MusicButton");
        MusicButtonOn = MusicButton.FindChild("On", true);
        MusicButtonOff = MusicButton.FindChild("Off", true);
        MusicButton.OnClick(ToggleMusicAction);

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
        bool music = PlayerPrefsHelper.GetBool(PlayerPrefsHelper.MUSIC);
        SoundButtonOn.SetActive(sound);
        SoundButtonOff.SetActive(!sound);
        MusicButtonOn.SetActive(music);
        MusicButtonOff.SetActive(!music);
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
        if (MusicAudioSource)
            SoundAudioSource.mute = state;
        UpdateUI();
    }

    internal void ToggleMusicAction()
    {
        bool state = PlayerPrefsHelper.GetBool(PlayerPrefsHelper.MUSIC);
        PlayerPrefsHelper.SetBool(PlayerPrefsHelper.MUSIC, !state);
        if(MusicAudioSource)
            MusicAudioSource.mute = state;
        UpdateUI();
    }

}


