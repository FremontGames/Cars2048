using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Commons.Lang;

using SimpleJSON;

namespace Commons.UI
{
    class TranslateUIProvider
    {
        public string PreferredLanguage = "en";
        public string FallbackLanguage = "en";
        public string FilesLoaderPrefix = "i18n/locale-";
        TranslateProvider core;
        public Language[] Languages;

        public void Initialize()
        {
            // TODO load preference
            core = new TranslateProvider();
            core.PreferredLanguage = PreferredLanguage;
            core.FallbackLanguage = FallbackLanguage;
            core.Translations("en", LoadFile("en"));
            core.Translations(PreferredLanguage, LoadFile(PreferredLanguage));
            core.Translations(FallbackLanguage, LoadFile(FallbackLanguage));

            InitializeLocaleLabels();
        }

        string LoadFile(string language)
        {
            string res;
            string path = FilesLoaderPrefix + language;
            TextAsset txt = Resources.Load<TextAsset>(path);
            if (txt != null)
            {
                res = txt.text;
            }
            else
            {
                res = null;
            }
            return res;
        }

        public void Apply(GameObject go, bool includeInactive = false, int depth = 10)
        {
            if (depth <= 0)
                return;
            string name = go.name.ToLower();
            string key;
            if (name.Contains("i18n="))
            {
                string[] res = Regex.Split(name, "i18n=");
                res = res[1].Split(' ');
                key = res[0].ToUpper();
                Apply(go, key);
            }
            if (go.transform.childCount > 0)
                for (int i = 0; i < go.transform.childCount; i++)
                    Apply(
                        go.transform.GetChild(i).gameObject,
                        includeInactive, depth - 1);
        }

        void Apply(GameObject go, string key)
        {
            string value = null;
            Text text;
            if (key == null)
                return;
            if (go.GetComponent<Text>() != null)
            {
                value = core.Translate(key);
                if (value == null)
                    return;
                text = go.GetComponent<Text>();
                text.text = value;
            }
        }

        private void InitializeLocaleLabels()
        {
            Languages = new Language[0];

            // load
            string path = "i18n/" + "locales";
            TextAsset txt = Resources.Load<TextAsset>(path);
            if (txt == null) return;
            string json = txt.text;
            if (json == null) return;
            if (json.Equals("")) return;
            JSONNode data = JSON.Parse(json);

            // get langs
            JSONArray array = data.AsArray;
            Languages = new Language[array.Count];
            for (int i = 0; i < array.Count; i++)
            {
                JSONNode node = array[i];
                Languages[i] = new Language()
                {
                    code = node["code"],
                    label = node["label"]
                };
            }
        }

        public Language Language(string code)
        {
            foreach (Language lang in Languages)
                if (code.ToLower().Equals(lang.code.ToLower()))
                    return lang;
            return null;
        }

    }

    public class Language
    {
        public string code;
        public string label;
    }

    public static class GameObjectExtensions
    {
        public static string ISO(this SystemLanguage systemLanguage)
        {
            string res = "EN";
            switch (systemLanguage)
            {
                case SystemLanguage.Afrikaans: res = "AF"; break;
                case SystemLanguage.Arabic: res = "AR"; break;
                case SystemLanguage.Basque: res = "EU"; break;
                case SystemLanguage.Belarusian: res = "BY"; break;
                case SystemLanguage.Bulgarian: res = "BG"; break;
                case SystemLanguage.Catalan: res = "CA"; break;
                case SystemLanguage.Chinese: res = "ZH"; break;
                case SystemLanguage.ChineseTraditional: res = "ZH-TW"; break;
                case SystemLanguage.Czech: res = "CS"; break;
                case SystemLanguage.Danish: res = "DA"; break;
                case SystemLanguage.Dutch: res = "NL"; break;
                case SystemLanguage.English: res = "EN"; break;
                case SystemLanguage.Estonian: res = "ET"; break;
                case SystemLanguage.Faroese: res = "FO"; break;
                case SystemLanguage.Finnish: res = "FI"; break;
                case SystemLanguage.French: res = "FR"; break;
                case SystemLanguage.German: res = "DE"; break;
                case SystemLanguage.Greek: res = "EL"; break;
                case SystemLanguage.Hebrew: res = "IW"; break;
                case SystemLanguage.Icelandic: res = "IS"; break;
                case SystemLanguage.Indonesian: res = "IN"; break;
                case SystemLanguage.Italian: res = "IT"; break;
                case SystemLanguage.Japanese: res = "JA"; break;
                case SystemLanguage.Korean: res = "KO"; break;
                case SystemLanguage.Latvian: res = "LV"; break;
                case SystemLanguage.Lithuanian: res = "LT"; break;
                case SystemLanguage.Norwegian: res = "NO"; break;
                case SystemLanguage.Polish: res = "PL"; break;
                case SystemLanguage.Portuguese: res = "PT"; break;
                case SystemLanguage.Romanian: res = "RO"; break;
                case SystemLanguage.Russian: res = "RU"; break;
                case SystemLanguage.SerboCroatian: res = "SH"; break;
                case SystemLanguage.Slovak: res = "SK"; break;
                case SystemLanguage.Slovenian: res = "SL"; break;
                case SystemLanguage.Spanish: res = "ES"; break;
                case SystemLanguage.Swedish: res = "SV"; break;
                case SystemLanguage.Thai: res = "TH"; break;
                case SystemLanguage.Turkish: res = "TR"; break;
                case SystemLanguage.Ukrainian: res = "UK"; break;
                case SystemLanguage.Unknown: res = "EN"; break;
                case SystemLanguage.Vietnamese: res = "VI"; break;
            }
            //		Debug.Log ("Lang: " + res);
            return res;
        }
    }
}
