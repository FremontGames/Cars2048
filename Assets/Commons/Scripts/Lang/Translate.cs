using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimpleJSON;

namespace Commons.Lang
{
    class TranslateProvider
    {
        public string PreferredLanguage = "en";
        public string FallbackLanguage = "en";

        Dictionary<string, JSONNode> locales;

        public TranslateProvider()
        {
            locales = new Dictionary<string, JSONNode>();
        }

        public void Translations(string ISOCode, string json)
        {
            JSONNode res;
            if (json == null) return;
            if (json.Equals("")) return;
            res = JSON.Parse(json);
            if (locales.ContainsKey(ISOCode))
                locales.Remove(ISOCode);
            locales.Add(ISOCode, res);
        }

        public string Translate(string key)
        {
            string value;
            JSONNode preferred = locales[PreferredLanguage];
            JSONNode fallback = locales[FallbackLanguage];
            if (preferred != null)
            {
                value = preferred[key].Value;
                if ("".Equals(value) && fallback != null)
                    value = fallback[key].Value;
            }
            else if (fallback != null)
                value = fallback[key].Value;
            else
                value = null;
            if ("".Equals(value))
                value = null;
            return value;
        }
    }
}
