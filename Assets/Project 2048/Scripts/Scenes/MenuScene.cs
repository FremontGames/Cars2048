using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Commons;
using Commons.UI;
using Commons.Animations;
using Project2048;

class MenuSceneState
{
    public int coinsCount = 0;
}

class MenuScene : MonoBehaviour
{
    MenuSceneState state;
    Main Main;
    GameObject CardDialog;

    void Start()
    {
        Application.targetFrameRate = Globals.QUALITY_FRAMERATE;
        QualitySettings.vSyncCount = Globals.QUALITY_VSYNC;
        Main = Main.Instance;
        state = new MenuSceneState()
        {
            coinsCount = Main.CountCoins()
        };
        // BINDING
        gameObject.FindChild("AchiementCountText")
            .GetComponent<Text>()
                .text = state.coinsCount.ToString();
        CardDialog = Instantiate(
            Resources.Load("Prefabs/CardDialog", typeof(GameObject)),
            GameObject.Find("UICanvas").transform
            ) as GameObject;
        CardDialog.SetActive(false);
        CardDialog.AddComponent<CardDialog>();
        gameObject.FindChild("GameHelpButton").OnClick(HelpOpenAction);
        // INITIALIZE
        buildMap();
        Main.Lang.Apply(GameObject.Find("UICanvas"), true);
        Main.Theme.Apply(GameObject.Find("UICanvas"), true);
        // ANIMATIONS
        ScreenTransitionAnimator.InitShade(
           Instantiate(new GameObject("shade"), GameObject.Find("UICanvas").transform))
               .AddComponent<ScreenOpenAnimator>();
        InitAudio();
    }

    void Update()
    {
        AppQuit.QuitIfEscape();
    }

    private void ShowHelpDialog()
    {

    }

    private void InitAudio()
    {
        AudioSource SoundAudioSource = gameObject.AddComponent<AudioSource>();
        SoundAudioSource.mute = !PlayerPrefsHelper.GetBool(PlayerPrefsHelper.SOUND);
        AudioSource MusicAudioSource = gameObject.AddComponent<AudioSource>();
        MusicAudioSource.mute = !PlayerPrefsHelper.GetBool(PlayerPrefsHelper.MUSIC);

        SoundAudio SoundAudio = gameObject.AddComponent<SoundAudio>();
        SoundAudio.AudioSource = SoundAudioSource;
        SoundAudio.PlayIntroAtStartup = true;
        MusicAudio MusicAudio = gameObject.AddComponent<MusicAudio>();
        MusicAudio.AudioSource = MusicAudioSource;

        SettingsButton SettingsButton = gameObject.FindChild("SettingsButton")
            .AddComponent<SettingsButton>();
        SettingsButton.TranslateTarget = GameObject.Find("UICanvas");
        SettingsButton.SoundAudioSource = SoundAudioSource;
        SettingsButton.MusicAudioSource = MusicAudioSource;
    }

    internal void BackAction()
    {
        SceneManager.LoadScene(Globals.SCENE_MAIN, LoadSceneMode.Single);
    }

    internal void StartAction(ChapterData level)
    {
        if (level == null)
            return;
        Globals.LEVEL = level;
        SceneManager.LoadScene(Globals.SCENE_GAME, LoadSceneMode.Single);
    }

    // ********************************************************************

    private void buildMap()
    {
        GameObject ChapPrefab = gameObject.FindChild("ChapterPrefab");
        ChapPrefab.SetActive(false);
        Transform parent = ChapPrefab.transform.parent.gameObject.transform;
        StoryData story = Main.Levels;
        int i = 0;
        GameObject go;
        foreach (ActData act in story.acts)
        {
            foreach (ChapterData chap in act.chapters)
            {
                int level_score = PlayerPrefs.GetInt(PlayerPrefsHelper.LEVEL + chap.id);
                go = Instantiate(ChapPrefab, parent);
                go.FindChild("LockedValueText").GetComponent<Text>()
                    .text = (chap.require * Globals.ACHIV_COINS_VALUE).ToString();
                go.FindChild("ObjectiveValueText").GetComponent<Text>()
                    .text = level_score.ToString();
                go.SetActive(true);
                bool isLocked = state.coinsCount < chap.require;
                if (isLocked)
                    DisableLevel(go);
                else
                {
                    EnableLevel(go, chap);
                    bool isNotDone = (level_score == 0);
                    if (isNotDone)
                        UndoneLevel(go);
                    else
                    {
                        StartedLevel(go,
                            chap.path + "/" + Globals.VALUES_TO_SPRITES[level_score],
                            new CardView()
                            {
                                id = chap.id,
                                path = chap.path,
                                objective = level_score,
                            });
                        bool isDone = (level_score >= chap.objective);
                        if (isDone)
                            DoneLevel(go);
                    }
                }
            }
            i++;
        }
    }

    private void DisableLevel(GameObject go)
    {
        go.FindChild("TileImage").SetActive(false);
        go.FindChild("QuestionImage").SetActive(true);
        go.name = go.name.Replace("style=", "style=disabled_");
        go.FindChild("StartButton").SetActive(false);
        go.FindChild("ObjectiveImage").SetActive(false);
    }

    private void EnableLevel(GameObject go, ChapterData level)
    {
        FocusLevel(go);
        go
            .AddComponent<Button>()
                .onClick.AddListener(
                    () => StartAction(level));
        go.FindChild("StartButton")
            .GetComponent<Button>()
                .onClick.AddListener(
                    () => StartAction(level));
        GameObject go2;
        go2= go.FindChild("PlayButton");
        go2.name = go2.name.Replace("style=", "style=primary_");
        go.FindChild("Money").SetActive(false);
    }

    private void UndoneLevel(GameObject go)
    {
        go.FindChild("TileImage", true).SetActive(false);
        go.FindChild("QuestionImage", true).SetActive(true);
    }

    private void StartedLevel(GameObject go, string spritePath, CardView card)
    {
        go.FindChild("TileImage").SetActive(true);
        go.FindChild("QuestionImage").SetActive(false);
        go.FindChild("TileImage")
            .GetComponent<Image>()
                .sprite = Resources.Load<Sprite>(spritePath);
    }

    void FocusLevel(GameObject go)
    {
        go.FindChild("PlayButton", true)
            .AddComponent<TiltAnimator>()
                .ScaleMax = 1.20f;
    }

    private void DoneLevel(GameObject go)
    {
        // TODO go.FindChild("DoneImage").SetActive(true);
    }

    internal void HelpOpenAction()
    {
        gameObject
            .FindChild("HelpDialog", true)
            .SetActive(true);
    }
}

