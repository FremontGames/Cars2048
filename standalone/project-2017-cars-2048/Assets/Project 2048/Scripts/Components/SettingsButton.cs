using UnityEngine;
using UnityEditor;
using Commons;
using Project2048;
using System;

public class SettingsButton : MonoBehaviour
{
    public GameObject TranslateTarget;
    public AudioSource SoundAudioSource;
    public AudioSource MusicAudioSource;
    SettingsPrefab settings;

    void Start()
    {
        LoadDialogPrefab();
        InitDialogPrefab();
        BindActions();
    }

    private void LoadDialogPrefab()
    {
        GameObject prefabGO = Instantiate(
            Resources.Load("Prefabs/SettingsPrefab", typeof(GameObject)),
            TranslateTarget.transform
        ) as GameObject;
        settings = prefabGO.AddComponent<SettingsPrefab>();
        settings.TranslateTarget = TranslateTarget;
    }

    private void BindActions()
    {
        gameObject
            .OnClick(OpenDialogAction);
    }

    internal void OpenDialogAction()
    {
        settings.gameObject
            .FindChild("SettingsDialog", true)
            .SetActive(true);
    }

    private void InitDialogPrefab()
    {
        settings.SoundAudioSource = SoundAudioSource;
        settings.MusicAudioSource = MusicAudioSource;
        settings.ShowHelpAtStartup = PlayerPrefsHelper.GetBool(PlayerPrefsHelper.HELP_AT_STARTUP);
    }
}