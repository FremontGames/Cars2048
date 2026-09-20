using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Commons;
using Commons.Lang;
using Commons.UI;
using SimpleJSON;
using Project2048;

// API ************************************************
public interface IMain
{
    int CountCoins();
    int CountCards();
    int CountCardsTotal();
    ChapterData FirstLevel();
    ChapterData LastPlayedLevel();
    ChapterData NextLevel(ChapterData actualLevel);
}

class Main: IMain
{
    public GameManager Core;
    public IStyleProvider Theme;
    public TranslateUIProvider Lang;
    public PlayerPrefsHelper Save;
    public AdMobHelper Ads;
    public StoryData Levels;

    // SINGLETON PATTERN **********************************

    private static Main singleton;
    internal static Main Instance
    {
        get
        {
            if (singleton == null)
                singleton = new Main();
            return singleton;
        }
    }

    // IMPL ************************************************

    private Main()
    {
        // UNITY3D
        Application.targetFrameRate = Globals.QUALITY_FRAMERATE;
        QualitySettings.vSyncCount = Globals.QUALITY_VSYNC;
        Ads = new AdMobHelper();
        Ads.Initialize("admob");

        // DATA
        Save = new PlayerPrefsHelper();
        Save.LoadThenInitializeIfNeeded();

        // GAME
        Core = new GameManager();
        Theme = InitThemeProvider();
        Lang = InitTranslateProvider();
        Levels = new LevelDataLoader().Load();
    }

    private IStyleProvider InitThemeProvider()
    {
        IStyleProvider provider = new StyleProvider();

        ButtonStyle raised = LoadButton("square-rounded-32");
        provider.ConfigButton("RaisedSmall", raised);
        provider.ConfigButton("Raised", raised);
        provider.ConfigButton("RaisedLarge", raised);
        provider.ConfigButton("RaisedExtralarge", raised);
        provider.ConfigButton("FABSmall", LoadButton("circle-32"));
        provider.ConfigButton("FAB", LoadButton("circle-64"));
        provider.ConfigButton("FABLarge", LoadButton("circle-128"));
        provider.ConfigButton("FABExtralarge", LoadButton("circle-256"));

        ThemeDatas themes = new ThemeDataloader().Load();
        foreach (ThemeData t in themes.themes)
            provider.ConfigTheme(t.name, new Theme()
            {
                Text = ColorHelper.HEXToRGB(t.Text),
                Background = ColorHelper.HEXToRGB(t.Background),
                Primary = ColorHelper.HEXToRGB(t.Primary),
                Warning = ColorHelper.HEXToRGB(t.Warning),
                Accent = ColorHelper.HEXToRGB(t.Accent),
            });
        Theme defaul = provider.GetTheme(Globals.THEME);
        provider.ConfigTheme("default", defaul);

        return provider;
    }

    private ButtonStyle LoadButton(string file)
    {
        return new ButtonStyle()
        {
            Sprite = Resources.Load<Sprite>("Sprites/" + file)
        };
    }

    private TranslateUIProvider InitTranslateProvider()
    {
        string save = PlayerPrefs.GetString(PlayerPrefsHelper.LANG);

        TranslateUIProvider t = new TranslateUIProvider();
        if (Strings.IsNotNullOrEmpty(save))
            t.PreferredLanguage = save;
        else if (Application.systemLanguage.ISO() != null)
            t.PreferredLanguage = Application.systemLanguage.ISO();

        t.Initialize();
        return t;
    }

    public bool MustStartTutorial()
    {
        int completion = Levels.acts[0].chapters[0].objective;
        int level = Core.ToTileIndex(completion);
        int objectiveCoins = level * Globals.ACHIV_COINS_VALUE;
        return CountCoins() < objectiveCoins;
    }

    public int CountCoins()
    {
        int count = 0;
        foreach (ActData act in Levels.acts)
            foreach (ChapterData chap in act.chapters)
            {
                int level_completion = PlayerPrefs.GetInt(PlayerPrefsHelper.LEVEL + chap.id);
                int level_com_index = Core.ToTileIndex(level_completion);
                count += level_com_index * Globals.ACHIV_COINS_VALUE;
            }
        return count;
    }

    public int CountCards()
    {
        int count = 0;
        CardView[] cards = CardDataLoader.LoadAll();
        foreach (CardView card in cards)
            if (!card.locked)
                count++;
        return count;
    }

    public int CountCardsTotal()
    {
        int count = 0;
        foreach (ActData act in Levels.acts)
            if (act.id != 0)
                foreach (ChapterData chap in act.chapters)
                    count++; ;
        return count * Globals.CARDS_BY_LEVEL;
    }

    public ChapterData TutorialLevel()
    {
        return Levels.acts[0].chapters[0];
    }

    public ChapterData FirstLevel()
    {
        return Levels.acts[1].chapters[0];
    }

    public ChapterData LastPlayedLevel()
    {
        int last = PlayerPrefs.GetInt(PlayerPrefsHelper.LAST_PLAYED);
        foreach (ActData act in Levels.acts)
            foreach (ChapterData chap in act.chapters)
                if(chap.id == last)
                    return chap;
        return TutorialLevel();
    }

    public ChapterData[] GetAllChapters()
    {
        List<ChapterData> chapters = new List<ChapterData>();
        foreach (ActData act in Levels.acts)
            foreach (ChapterData chap in act.chapters)
            {
                chapters.Add(chap);
            }
        return chapters.ToArray();
    }

    public ChapterData NextLevel(ChapterData actualLevel)
    {
        ChapterData[] chapters = GetAllChapters();
        for (int i = 0; i < chapters.Length; i++)
            if ((chapters[i].id == actualLevel.id) && (i < (chapters.Length-1)))
                return chapters[i + 1];
        return FirstLevel();
    }
}
