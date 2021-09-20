using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Commons;
using Commons.UI;

class SettingsPrefab : MonoBehaviour
{
    public GameObject TranslateTarget;
    public AudioSource SoundAudioSource;
    public AudioSource MusicAudioSource;
    public Boolean ShowHelpAtStartup;

    private void Start()
    {
        GameObject HelpDialog1 = gameObject.FindChild("HelpDialog1", true);
        GameObject HelpDialog2 = gameObject.FindChild("HelpDialog2", true);
        GameObject HelpDialog3 = gameObject.FindChild("HelpDialog3", true);
        GameObject HelpDialog4 = gameObject.FindChild("HelpDialog4", true);
        HelpDialog1.SetActive(false);
        HelpDialog2.SetActive(false);
        HelpDialog3.SetActive(false);
        HelpDialog4.SetActive(false);
        HelpDialog1.AddComponent<BringToFront>();
        HelpDialog2.AddComponent<BringToFront>();
        HelpDialog3.AddComponent<BringToFront>();
        HelpDialog4.AddComponent<BringToFront>();
        NextDialog h1 = HelpDialog1.AddComponent<NextDialog>();
        h1.Next = HelpDialog2;
        NextPrevDialog h2 = HelpDialog2.AddComponent<NextPrevDialog>();
        h2.Prev = HelpDialog1;
        h2.Next = HelpDialog3;
        NextPrevDialog h3 = HelpDialog3.AddComponent<NextPrevDialog>();
        h3.Prev = HelpDialog2;
        h3.Next = HelpDialog4;
        PrevDialog h4 = HelpDialog4.AddComponent<PrevDialog>();
        h4.Prev = HelpDialog3;

        GameObject LanguageDialog = gameObject.FindChild("LanguageDialog", true);
        LanguageDialog.SetActive(false);
        LanguageDialog.AddComponent<BringToFront>();
        LanguageDialog lang = LanguageDialog.AddComponent<LanguageDialog>();
        lang.Translate = Main.Instance.Lang;
        lang.Target = TranslateTarget;

        GameObject them = gameObject.FindChild("ThemeDialog", true);
        them.SetActive(false);
        them.AddComponent<BringToFront>();
        them.AddComponent<ThemeDialog>()
            .Target = TranslateTarget;

        GameObject SettingsDialog = gameObject.FindChild("SettingsDialog", true);
        SettingsDialog.SetActive(false);
        SettingsDialog.AddComponent<BringToFront>();
        SettingsDialog script = SettingsDialog.AddComponent<SettingsDialog>();
        script.LanguageDialog = LanguageDialog;
        script.HelpDialog = HelpDialog1;
        script.ThemeDialog = them;
        script.SoundAudioSource = SoundAudioSource;
        script.MusicAudioSource = MusicAudioSource;
        SettingsDialog.FindChild("ApplicationVersionText", true)
            .GetComponent<Text>()
                .text = Application.version;

        if (ShowHelpAtStartup)
            HelpDialog1.SetActive(true);
    }

}

