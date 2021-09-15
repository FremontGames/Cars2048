using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Globalization;
using System.Text.RegularExpressions;

// https://support.unity3d.com/hc/en-us/articles/208456906-Excluding-Scripts-and-Assets-from-builds
#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace Commons.UI
{
    // API ********************************************************************

    interface IStyleProvider
    {
        void Apply(GameObject go, bool includeInactive = false, int depth = 10, string theme = "default", string typo = "default");

        void ConfigTheme(string name, Theme theme);
        Theme GetTheme(string theme = "default");
        string[] GetThemes();

        void ConfigTypo(string name, Typography typo);
        Typography GetTypo(string typo = "default");

        void ConfigButton(string name, ButtonStyle button);
    }

    class Theme
    {
        public Color Text;
        public Color Background;

        public Color Warning;
        public Color Primary;
        public Color Accent;
    }

    class Script
    {
        public int FontSize;
        public FontStyle FontStyle;
        public bool Caps;
        public double ContrastRatio;
    }

    class Typography
    {
        // public Font font;

        public Script caption;
        public Script button;
        public Script body1;
        public Script body2;

        public Script subhead;
        public Script title;
        public Script headline;
        public Script display1;
        // public Script display2;
        // public Script display3;
        // public Script display4;
    }

    class ButtonStyle
    {
        public Sprite Sprite;
    }

#if (UNITY_EDITOR)
    // EDITOR TOOLS ***********************************************************

    // http://docs.unity3d.com/Manual/RunningEditorCodeOnLaunch.html
    [InitializeOnLoad]
    public class Startup
    {
        // https://unity3d.com/fr/learn/tutorials/topics/interface-essentials/unity-editor-extensions-menu-items

        [MenuItem("Tools/UI Style/Apply on selection %&s")]
        private static void ApplyThemeOption()
        {
            var selected = Selection.activeObject;
            if (selected == null)
            {
                Debug.Log("ERROR: Please select something!");
                return;
            }
            if (selected.GetType() != typeof(GameObject))
            {
                Debug.Log("ERROR: Please select a GameObject!");
                return;
            }
            new StyleProvider().Apply((GameObject)selected);
        }
    }
#endif

    // IMPL *******************************************************************

    class StyleProvider : IStyleProvider
    {
        // public Sprite RoundSprite;
        // public Sprite RoundSpriteSmall;
        // public Sprite RoundSpriteLarge;
        // public Sprite CircleSprite;
        // public Sprite CircleSpriteSmall;
        // public Sprite CircleSpriteLarge;

        Dictionary<string, Typography> typos;
        Dictionary<string, Theme> themes;
        Dictionary<string, ButtonStyle> buttons;
        ComponentHelper comp = new ComponentHelper();
        TypoHelper typo = new TypoHelper();

        public StyleProvider()
        {
            themes = new Dictionary<string, Theme>();
            themes.Add("default", defaultTheme);
            typos = new Dictionary<string, Typography>();
            typos.Add("default", defaultTypo);
            buttons = new Dictionary<string, ButtonStyle>();
        }
        public void ConfigButton(string name, ButtonStyle button)
        {
            if (buttons.ContainsKey(name))
                buttons[name] = button;
            else
                buttons.Add(name, button);
        }
        public void ConfigTheme(string name, Theme theme)
        {
            if (themes.ContainsKey(name))
                themes[name] = theme;
            else
                themes.Add(name, theme);
        }
        public Theme GetTheme(string name = "default")
        {
            if (themes.ContainsKey(name))
                return themes[name];
            else
                return themes["default"];
        }
        public string[] GetThemes()
        {
            return themes.Keys.ToArray();
        }
        public void ConfigTypo(string name, Typography typo)
        {
            typos.Add(name, typo);
        }
        public Typography GetTypo(string name = "default")
        {
            return typos[name];
        }

        public void Apply(GameObject go, bool includeInactive = false, int depth = 20, string theme = "default", string typo = "default")
        {
            if (depth <= 0)
                return;
            string name = go.name.ToLower();
            if (name.Contains("theme="))
            {
                string[] res = Regex.Split(name, "theme=");
                res = res[1].Split(' ');
                theme = res[0];
            }
            if (name.Contains("typo="))
            {
                typo = "default";
            }
            ApplyTypo(go, typo);
            ApplyTheme(go, theme);
            if (go.transform.childCount > 0)
                for (int i = 0; i < go.transform.childCount; i++)
                    Apply(
                        go.transform.GetChild(i).gameObject,
                        includeInactive, depth - 1,
                        theme, typo);
        }

        private void ApplyTypo(GameObject go, string typo = "default")
        {
            if (go.GetComponent<Text>() == null)
                return;

            Typography t = typos[typo];

            var parent = go.transform.parent.gameObject;
            string[] parentTags = StyleHelper.GetTagsFromName(parent, "style=");
            if (parentTags.Contains("button"))
                this.typo.ApplyFont(go, t.button ?? t.body1);

            string[] tags = StyleHelper.GetTagsFromName(go, "style=");

            float scale = (parentTags.Contains("large") || parentTags.Contains("small")) ?
                    BuildScale(parentTags) :
                    BuildScale(tags);

            if (tags.Contains("button"))
                this.typo.ApplyFont(go, t.button ?? t.body1);
            else if (tags.Contains("title"))
                this.typo.ApplyFont(go, t.title ?? t.body1);
            else if (tags.Contains("display1"))
                this.typo.ApplyFont(go, t.display1 ?? t.body1);
            else if (tags.Contains("headline"))
                this.typo.ApplyFont(go, t.headline ?? t.body1);
            else if (tags.Contains("body"))
                this.typo.ApplyFont(go, t.body1);
            else if (tags.Contains("body1"))
                this.typo.ApplyFont(go, t.body1);
            else if (tags.Contains("body2"))
                this.typo.ApplyFont(go, t.body2);

            this.comp.ApplyTextSize(go, scale);
        }

        private void ApplyTheme(GameObject go, string theme = "default")
        {
            string[] tags = StyleHelper.GetTagsFromName(go, "style=");

            Theme them = themes.ContainsKey(theme) ? themes[theme] : themes["default"];
            Theme[] colors = StyleHelper.BuildIntentionColors(tags, them);
            Theme defaul = colors[0];
            Theme invers = colors[1];
            Color shadowColor = new Color(0, 0, 0, 0.4f);
            /*
            if (tags.Length > 0)
            {
                comp.ApplyText(go, them.Text);
            }
            */
            float scale = BuildScale(tags);

            if (tags.Contains("button"))
            {
                comp.ApplyTextSize(go, scale);
                if (tags.Contains("raised"))
                {
                    comp.ApplySpriteRound(go, SpriteScaled("Raised", tags));
                    comp.ApplyBackgroundColor(go, invers.Background);
                    comp.ApplyRaised(go, shadowColor);
                    comp.ApplySize(go, -1, 36 * scale);
                    // CHILD
                    comp.ApplyText(go, invers.Text);
                }
                else if (tags.Contains("fab"))
                {
                    comp.ApplySpriteCircle(go, SpriteScaled("FAB", tags));
                    comp.ApplyBackgroundColor(go, invers.Background);
                    comp.ApplyRaised(go, shadowColor);
                    comp.ApplySize(go, (56 * scale), (56 * scale));
                    // CHILD
                    comp.ApplyText(go, invers.Text);

                    Text text = go.GetComponentInChildren<Text>();
                    if (text != null)
                    {
                        GameObject gg = text.gameObject;
                        comp.ApplySize(gg,
                            (56 * scale) * 2,
                            (56 * scale) / 2);
                        gg.transform.localPosition = new Vector2(
                            0,
                            (56 * scale) * -0.75f);
                        go.GetComponentInChildren<Text>()
                            .alignment = TextAnchor.UpperCenter;
                    }
                }
                else // FLAT
                {
                    comp.ApplySpriteNone(go);
                    comp.ApplySize(go, -1, 36 * scale);
                    // CHILD
                    comp.ApplyText(go, defaul.Text);
                }
            }
            if (tags.Contains("icon"))
            {
                var parent = go.transform.parent.gameObject;
                string[] parentTags = StyleHelper.GetTagsFromName(parent, "style=");
                if (parentTags.Contains("raised") |
                     parentTags.Contains("fab"))
                {
                    Theme[] parentColors = StyleHelper.BuildIntentionColors(parentTags, them);
                    Theme parentInvers = parentColors[1];
                    float parentScale = BuildScale(parentTags);
                    comp.ApplyIcon(go, parentInvers.Text);
                    if (parentTags.Contains("large"))
                        comp.ApplySize(go, 24 * scale, 24 * scale);
                    else
                        comp.ApplySize(go, 24 * parentScale, 24 * parentScale);
                }
                else
                {
                    comp.ApplyIcon(go, defaul.Text);
                    comp.ApplySize(go, 24 * scale, 24 * scale);
                }
            }
            if (tags.Contains("content") |
                tags.Contains("headline") |
                tags.Contains("title") |
                tags.Contains("display1") |
                tags.Contains("body1") |
                tags.Contains("body2") |
                tags.Contains("body"))
            {
                var parent = go.transform.parent.gameObject;
                string[] parentTags = StyleHelper.GetTagsFromName(parent, "style=");
                Theme[] parentColors = StyleHelper.BuildIntentionColors(parentTags, them);
                Theme parentInvers = parentColors[1];
                if (parentTags.Contains("toolbar"))
                    comp.ApplyText(go, parentInvers.Text);
                else
                    comp.ApplyText(go, defaul.Text);
            }

            if (tags.Contains("disabled"))
            {
                float a = 0.9f;
                foreach (var img in go.GetComponentsInChildren<Image>())
                    img.color = new Color(them.Background.r, them.Background.g, them.Background.b,
                        a);
                foreach (var txt in go.GetComponentsInChildren<Text>())
                    txt.color = new Color(txt.color.r, txt.color.g, txt.color.b,
                        a);
                foreach (var btn in go.GetComponentsInChildren<UnityEngine.UI.Button>())
                    btn.interactable = false;
            }

            if (tags.Contains("background"))
            {
                comp.ApplyBackgroundColor(go, them.Background);
            }
            if (tags.Contains("list-item"))
            {
                comp.ApplyBackgroundColor(go, them.Background);
                comp.ApplySize(go, -1, 48);
            }
            if (tags.Contains("divider"))
            {
                comp.ApplyBackgroundColor(go, them.Text);
                comp.ApplySize(go, -1, 1);
            }
            if (tags.Contains("avatar"))
            {
                comp.ApplySize(go, 40, 40);
            }
            if (tags.Contains("toolbar"))
            {
                comp.ApplyBackgroundColor(go, them.Primary);
                comp.ApplySize(go, -1, 64);
                // CHILD
                comp.ApplyText(go, invers.Text);
            }
            if (tags.Contains("dialog"))
            {
                comp.ApplySize(go, 350, -1);
                comp.ApplyBackgroundColor(go, them.Background);
            }
        }

        private Sprite SpriteScaled(string preffix, string[] tags)
        {
            string key = BuildScaleName(preffix, tags);
            return (buttons.ContainsKey(key)) ?
                buttons[key].Sprite :
                null;
        }

        private float BuildScale(string[] tags)
        {
            if (tags.Contains("extraextralarge"))
                return 4f;
            else if (tags.Contains("extralarge"))
                return 2.0f;
            else if (tags.Contains("large"))
                return 1.5f;
            else if (tags.Contains("small"))
                return 0.5f;
            else return 1f;
        }
        private string BuildScaleName(string prefix, string[] tags)
        {
            if (tags.Contains("extralarge"))
                return prefix + "Extralarge";
            else if (tags.Contains("large"))
                return prefix + "Large";
            else if (tags.Contains("small"))
                return prefix + "Small";
            else return prefix;
        }

        // TODO LOAD JSON FILE
        static Theme defaultTheme = new Theme()
        {
            Text = ColorHelper.HEXToRGB("000000"),
            Background = ColorHelper.HEXToRGB("FFFFFF"),
            Primary = ColorHelper.HEXToRGB("0000FF"),
            Accent = ColorHelper.HEXToRGB("FFFF00"),
            Warning = ColorHelper.HEXToRGB("FF0000"),
        };
        static Typography defaultTypo = new Typography()
        {
            //  font = Resources.Load<Font>("Fonts/Roboto-Regular"),
            display1 = new Script()
            {
                FontSize = 34,
                FontStyle = FontStyle.Normal,
                Caps = false,
                ContrastRatio = 0.87,
            },
            headline = new Script()
            {
                FontSize = 24,
                FontStyle = FontStyle.Normal,
                Caps = false,
                ContrastRatio = 0.87,
            },
            title = new Script()
            {
                FontSize = 20,
                FontStyle = FontStyle.Bold,
                Caps = false,
                ContrastRatio = 0.87,
            },
            subhead = new Script()
            {
                FontSize = 16,
                FontStyle = FontStyle.Normal,
                Caps = false,
                ContrastRatio = 0.87,
            },

            body1 = new Script()
            {
                FontSize = 14,
                FontStyle = FontStyle.Normal,
                Caps = false,
                ContrastRatio = 0.87,
            },
            body2 = new Script()
            {
                FontSize = 15,
                FontStyle = FontStyle.Bold,
                Caps = false,
                ContrastRatio = 0.87,
            },
            button = new Script()
            {
                FontSize = 14,
                FontStyle = FontStyle.Bold,
                Caps = true,
                ContrastRatio = 1,
            },
            caption = new Script()
            {
                FontSize = 12,
                FontStyle = FontStyle.Normal,
                Caps = true,
                ContrastRatio = 1,
            },
        };
    }

    public static class ColorHelper
    {
        public static Color HEXToRGB(string hex)
        {
            if (hex.Length != 6)
                throw new ArgumentException("Length must be equal to 6");
            return new Color(
                int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber) / 255f,
                int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber) / 255f,
                int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber) / 255f);
        }

        public static Color ContrastRatio(Color color, float ContrastRatio)
        {
            float r = color.r * ContrastRatio;
            float g = color.g * ContrastRatio;
            float b = color.b * ContrastRatio;
            return new Color(r, g, b);
        }

        public static Color TransparencyRatio(Color color, float TransparencyRatio)
        {
            return new Color(color.r, color.g, color.b, TransparencyRatio);
        }
    }

    class TypoHelper
    {
        internal void ApplyFont(GameObject go, Script script)
        {
            Text txt;
            txt = go.GetComponent<Text>() ?? go.AddComponent<Text>();
            txt.text = (script.Caps) ?
                txt.text.ToUpper() :
                txt.text.Substring(0, 1).ToUpper() + txt.text.Substring(1);

            txt.fontSize = script.FontSize;
            txt.fontStyle = script.FontStyle;
        }
    }

    class ComponentHelper
    {
        internal void ApplySize(GameObject go, float width, float height)
        {
            RectTransform rect;
            rect = go.GetComponent<RectTransform>() ?? go.AddComponent<RectTransform>();
            if (width == -1)
                width = rect.sizeDelta.x;
            if (height == -1)
                height = rect.sizeDelta.y;
            rect.sizeDelta = new Vector2(width, height);
        }

        internal void ApplySpriteRound(GameObject go, Sprite sprite)
        {
            Image img;
            img = go.GetComponent<Image>() ?? go.AddComponent<Image>();
            img.sprite = sprite;
            img.type = Image.Type.Sliced;
        }

        internal void ApplySpriteCircle(GameObject go, Sprite sprite)
        {
            Image img;
            img = go.GetComponent<Image>() ?? go.AddComponent<Image>();
            img.sprite = sprite;
            img.type = Image.Type.Simple;
        }

        internal void ApplyBackgroundColor(GameObject go, Color color)
        {
            Image img;
            img = go.GetComponent<Image>() ?? go.AddComponent<Image>();
            img.color = color;
        }

        internal void ApplyOutline(GameObject go, Color color)
        {
            Outline ou;
            ou = go.GetComponent<Outline>() ?? go.AddComponent<Outline>();
            ou.effectColor = color;
            ou.effectDistance = new Vector2(0, 1);
        }

        internal void ApplySpriteNone(GameObject go)
        {
            if (go.GetComponent<Text>() != null)
                return;
            Image img;
            img = go.GetComponent<Image>() ?? go.AddComponent<Image>();
            img.color = new Color(0, 0, 0, 0);
        }

        internal void ApplyRaised(GameObject go, Color color)
        {
            foreach (Shadow oldSha in go.GetComponents<Shadow>())
                UnityEngine.Object.Destroy(oldSha);

            Shadow sha;
            sha = go.AddComponent<Shadow>();
            sha.effectDistance = new Vector2(2, -2);
            sha.effectColor = color;

            sha = go.AddComponent<Shadow>();
            sha.effectDistance = new Vector2(-0.2f, 0.3f);
            sha.effectColor = color;
        }

        internal void ApplyIcon(GameObject go, Color color)
        {
            Image img;
            img = go.GetComponent<Image>() ?? go.AddComponent<Image>();
            img.color = color;
        }

        internal void ApplyText(GameObject go, Color color)
        {
            if (go.GetComponentInChildren<Text>() == null)
                return;
            foreach (Text txt in go.GetComponentsInChildren<Text>())
                txt.color = color;
        }
        internal void ApplyTextSize(GameObject go, float scale)
        {
            Text txt;
            txt = go.GetComponent<Text>();
            if (txt == null)
                return;
            txt.fontSize = (int)(txt.fontSize * scale);
        }
    }

    class StyleHelper
    {
        internal static string[] GetTagsFromName(GameObject go, string prefix)
        {
            if (go == null)
                return new string[0];

            string tagsAsString = go.name.ToLower();

            string foundedUIPart = null;
            string[] strParts = tagsAsString.ToLower().Split(' ');
            foreach (string str in strParts)
                if (str.StartsWith(prefix))
                    foundedUIPart = str;
            if (foundedUIPart == null)
                return new string[0];

            foundedUIPart = foundedUIPart.Replace(prefix, "");
            return foundedUIPart.Split('_');
        }

        internal static Color Inverse(Color back, Color front)
        {
            Color black = new Color(0, 0, 0);
            Color white = new Color(1, 1, 1);

            float frontScore = front.r + front.g + front.b;
            float backScore = back.r + back.g + back.b;
            float score = frontScore - backScore;
            bool NotDarkEnough = (0 < score) && (score < 0.5);
            bool NotLightEnough = (-0.5 < score) && (score < 0);

            if (NotLightEnough)
                front = white;
            if (NotDarkEnough)
                front = black;

            return front;
        }

        internal static Theme[] BuildIntentionColors(string[] tags, Theme theme)
        {
            Color transparent = new Color(0, 0, 0, 0);
            Theme[] themes;
            if (tags.Contains("warning"))
                themes = new Theme[2] {
                    new Theme()
                    {
                        Text = theme.Warning,
                        Background = transparent,
                    },
                     new Theme()
                    {
                        Text = Inverse(theme.Warning, theme.Text),
                        Background = theme.Warning,
                    }};
            else if (tags.Contains("primary") | tags.Contains("toolbar"))
                themes = new Theme[2] {
                    new Theme()
                    {
                        Text = theme.Primary,
                        Background = transparent,
                    },
                     new Theme()
                    {
                        Text = Inverse(theme.Primary, theme.Text),
                        Background = theme.Primary,
                    }};
            else if (tags.Contains("accent"))
                themes = new Theme[2] {
                    new Theme()
                    {
                        Text = theme.Accent,
                        Background = transparent,
                    },
                     new Theme()
                    {
                        Text = Inverse(theme.Accent, theme.Text),
                        Background = theme.Accent,
                    }};
            else
                themes = new Theme[2] {
                    new Theme()
                    {
                        Text = theme.Text,
                        Background = theme.Background,
                    },
                     new Theme()
                    {
                        Text = theme.Text,
                        Background = theme.Background,
                    }};
            return themes;
        }
    }
}
