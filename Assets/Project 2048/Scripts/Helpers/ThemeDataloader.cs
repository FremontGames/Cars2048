using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class ThemeDatas
{
    public ThemeData[] themes;
}

[Serializable]
public class ThemeData
{
    public string name;
    public string Text;
    public string Background;
    public string Warning;
    public string Primary;
    public string Accent;
}

class ThemeDataloader
{
    public string JSONPath = "themes";

    public ThemeDatas Load()
    {
        TextAsset txt = Resources.Load<TextAsset>(JSONPath);
        ThemeDatas data = JsonUtility.FromJson<ThemeDatas>(txt.text);
        return data;
    }
}