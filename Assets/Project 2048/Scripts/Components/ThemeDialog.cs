using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Commons;
using Commons.UI;


class ThemeDialog : MonoBehaviour
{
    public GameObject Target;

    GameObject ValueText;
    GameObject TogglePrefab;
    GameObject[] Toggles;

    Main Main;

    void Start()
    {
        Main = Main.Instance;

        gameObject.FindChild("CloseButton")
            .OnClick(CloseAction);
        gameObject.FindChild("OkButton")
            .OnClick(CloseAction);

        TogglePrefab = gameObject.FindChild("ThemeTogglePrefab", true);
        ValueText = gameObject.FindChild("ThemeValueText");
        ValueText = gameObject.FindChild("ThemeValueText");
        BuildToggleList();
    }

    void OnEnable()
    {
        Main.Instance.Theme.Apply(gameObject);
        Main.Instance.Lang.Apply(gameObject, true);
    }

    internal void CloseAction()
    {
        gameObject.SetActive(false);
    }

    internal void SelectedAction(bool arg)
    {
        string selection = ToggleHelper.GetSelection(Toggles).name;

        // SPECIFIC
        string text = selection.Replace("Theme", "");
        ValueText.GetComponent<Text>().text = text;

        IStyleProvider sp = Main.Theme;
        Theme defaul = sp.GetTheme(selection);
        sp.ConfigTheme("default", defaul);
        sp.Apply(Target, true);

        PlayerPrefs.SetString(PlayerPrefsHelper.THEME, selection);
    }

    // INTERNAL *************************************************

    private void BuildToggleList()
    {
        // SPECIFIC
        string selected = Globals.THEME;
        string[] keys = Main.Theme.GetThemes();

        GameObject original = TogglePrefab;
        Transform parent = original.transform.parent;
        original.SetActive(false);
        List<GameObject> list = new List<GameObject>();
        foreach (string key in keys)
        {
            // SPECIFIC
            if (!key.ToLower().Contains("theme"))
                continue;
            string id = key;
            string text = key.Replace("Theme", "");
            bool state = selected.ToLower().Equals(key.ToLower());

            GameObject go = GameObject.Instantiate(original, parent);
            go.name = id;
            go.GetComponentInChildren<Text>().text = text;
            go.GetComponent<Toggle>().isOn = state;
            go.GetComponent<Toggle>().onValueChanged.AddListener(SelectedAction);
            go.SetActive(true);
            list.Add(go);

            // SPECIFIC
            Theme theme = Main.Theme.GetTheme(key);
            go.GetComponentInChildren<Text>().color = theme.Text;
            foreach (Image img in go.GetComponentsInChildren<Image>())
                img.color = theme.Background;
        }
        Toggles = list.ToArray();
        ValueText.GetComponent<Text>().text = selected.Replace("Theme", "");
    }

}
