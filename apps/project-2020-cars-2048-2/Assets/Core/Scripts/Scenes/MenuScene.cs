using System;
using System.Collections.Generic;
using System.Linq;
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
    
#if (UNITY_EDITOR)
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded() {
        Debug.Log("xxx");
    }
#endif

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

        SoundAudio SoundAudio = gameObject.AddComponent<SoundAudio>();
        SoundAudio.AudioSource = SoundAudioSource;
        SoundAudio.PlayIntroAtStartup = true;

        SettingsButton SettingsButton = gameObject.FindChild("SettingsButton")
            .AddComponent<SettingsButton>();
        SettingsButton.TranslateTarget = GameObject.Find("UICanvas");
        SettingsButton.SoundAudioSource = SoundAudioSource;
    }

    internal void StartAction(ChapterData level)
    {
        if (level == null)
            return;
        Globals.LEVEL = level;
        SceneManager.LoadScene(Globals.SCENE_GAME, LoadSceneMode.Single);
    }

    internal void NextAction()
    {
        if(index >= (levels.Count-1)) 
            return;
        levels.ElementAt(index).Key.SetActive(false);
        index++;
        levels.ElementAt(index).Key.SetActive(true);
        PrevButton.GetComponent<Button>().interactable = true;
        if(index >= (levels.Count-1)) 
            NextButton.GetComponent<Button>().interactable = false;
    }

    internal void PrevAction()
    {
        if(index <= 0) 
            return;
        levels.ElementAt(index).Key.SetActive(false);
        index--;
        levels.ElementAt(index).Key.SetActive(true);
        NextButton.GetComponent<Button>().interactable = true;
        if(index <= 0) 
            PrevButton.GetComponent<Button>().interactable = false;
    }

    // ********************************************************************

    Dictionary<GameObject, ChapterData> levels = new Dictionary<GameObject, ChapterData>();
    int index = 0;
    GameObject NextButton;
    GameObject PrevButton;

    private void buildMap()
    {
        NextButton = GameObject.Find("NextButton");
        PrevButton = GameObject.Find("PrevButton");
        GameObject unlock = GameObject.Find("UnlockedPrefab");
        GameObject locked = GameObject.Find("LockedPrefab");
        unlock.SetActive(false);
        locked.SetActive(false);
        Transform parent = unlock.transform.parent.gameObject.transform;
        StoryData story = Main.Levels;
        int i = 0;
        GameObject go;
        foreach (ActData act in story.acts)
        {
            foreach (ChapterData chap in act.chapters)
            {
                bool isLocked = state.coinsCount < chap.require;
                if (isLocked) {
                    go = Instantiate(locked, parent);
                    RenderLocked(go, chap);
                }
                else {
                    go = Instantiate(unlock, parent);
                    RenderUnlocked(go, chap);
                    if(i == 0)
                        go.SetActive(true);
                }
                go.name = "Level"+i;
                levels.Add(go, chap);
                i++;
            }
        }
        NextButton
            .GetComponent<Button>()
                .onClick.AddListener(
                    () => NextAction());
        PrevButton
            .GetComponent<Button>()
                .onClick.AddListener(
                    () => PrevAction());
        PrevButton
            .GetComponent<Button>()
                .interactable = false;
        for (int y = 0; y < levels.Count; y++)
        {
            string position = "";
            for (int z = 0; z < levels.Count; z++)
                position+= ((y == z) ? " o" : " -");
            go = levels.ElementAt(y).Key;
            go.FindChild("LevelPositionText").GetComponent<Text>()
                .text = position;
        }
    }

    private void RenderLocked(GameObject go, ChapterData chap)
    {
        int level_score = PlayerPrefs.GetInt(PlayerPrefsHelper.LEVEL + chap.id);
        go.FindChild("LockedValueText").GetComponent<Text>()
            .text = (chap.require * Globals.ACHIV_COINS_VALUE).ToString();
    }

    private void RenderUnlocked(GameObject go, ChapterData chap)
    {
        int level_score = PlayerPrefs.GetInt(PlayerPrefsHelper.LEVEL + chap.id);
        string spritePath = chap.path + "/" + Globals.VALUES_TO_SPRITES[level_score];
        go.SetActive(false);
        go.FindChild("ObjectiveValueText").GetComponent<Text>()
            .text = level_score.ToString() + " /2048";
        go.FindChild("TileImage")
            .GetComponent<Image>()
                .sprite = Resources.Load<Sprite>(spritePath);
        go.FindChild("StartButton")
            .GetComponent<Button>()
                .onClick.AddListener(
                    () => StartAction(chap));
        go.FindChild("StartButton")
            .AddComponent<TiltAnimator>().ScaleMax = 1.5f;

        LevelData levelData = JsonUtility.FromJson<LevelData>(
            Resources.Load<TextAsset>(
                chap.path + "/" + "data")
                    .text);
        Color color = ColorHelper.HEXToRGB(levelData.color);
        Sprite sprite = Resources.Load<Sprite>(chap.path + "/" + "wallpaper");
        go.FindChild("WallpaperImage").GetComponent<Image>().sprite = sprite;
        go.FindChild("Map0").GetComponent<Image>().color = color;
        go.FindChild("Map1").GetComponent<Image>().color = color;
    }

    internal void HelpOpenAction()
    {
        gameObject
            .FindChild("HelpDialog", true)
            .SetActive(true);
    }
}

