using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using Commons;
using Commons.Animations;
using Commons.UI;
using Commons.Inputs;
using Project2048;
using GoogleMobileAds.Api;

class GameSceneProps
{
    public int CameraSize = 550;
    public float CanvasScalerMatch = 0.5f;
    public float WinAnimationTime = 1.5f;
    public float MoveAnimationTime = 0.1f;
    public float WrongAnimationTime = 0.5f;
    public float BonusAnimationTime = 0.5f;
}

class GameSceneState
{
    public Dictionary<int, Sprite> TileSprites = new Dictionary<int, Sprite>();
    public Dictionary<int, Sprite> PreviewSprites = new Dictionary<int, Sprite>();
    public string levelId;
    public LevelData levelData;
}

public class LevelData
{
    public string color;
}

class GameScene : MonoBehaviour
{
    GameSceneProps props;
    GameSceneState state;
    GameSceneView view;
    Main Main;
    GameManager Core;
    Game Model;
    SoundAudio SoundAudio;
    GameObject TileMovePrefab;
    GameObject WinAnimation;
    GameObject CardDialog;
    InterstitialAd Ads;

    void Start()
    {
        Application.targetFrameRate = Globals.QUALITY_FRAMERATE;
        QualitySettings.vSyncCount = Globals.QUALITY_VSYNC;
        props = new GameSceneProps();
        state = new GameSceneState()
        {
            levelId = PlayerPrefsHelper.LEVEL + Globals.LEVEL.id
        };
        Main = Main.Instance;
        Core = Main.Core;
        PlayerPrefs.SetInt(PlayerPrefsHelper.LAST_PLAYED, Globals.LEVEL.id);
        LoadData();
        InitBindView();
        InitComponents();
        InitListeners();
        InitStyle();
        InitAnimations();
        InitActions();
        InitAds();
        InitAudio();
        Main.Ads.RequestBanner("BannerView Bottom");
    }

    void Update()
    {
        AppQuit.QuitIfEscape();
    }

    // ********************************************************************

    private void InitAds()
    {
        Ads = Main.Ads.RequestInterstitial("InterstitialAd Undo");
        Ads.OnAdClosed += HandleOnAdClosed;
    }



    internal void HelpOpenAction()
    {
        gameObject
            .FindChild("HelpDialog", true)
            .SetActive(true);
    }

    internal void InitActions()
    {
        LoadSprites();
        StartGame();
        InitSprites();
        UpdateScreenAfterAction();
    }

    internal void UndoAction()
    {
        if (Ads.IsLoaded())
            Ads.Show();
        Model = Core.Undo();
        UpdateScreenAfterAction();
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        IEnumerator coroutine = HandleOnAdClosedWaitRoutine();
        StartCoroutine(coroutine);
    }

    private IEnumerator HandleOnAdClosedWaitRoutine()
    {
        yield return new WaitForSeconds(Globals.TIME_BETWEEN_ADS);
        Ads.LoadAd(Main.Ads.BuildRequest());
    }

    internal void MoveAction(Movement move)
    {
        if (!Model.AvailableMoves.Contains(move))
        {
            StartMoveWrongAnimation(move);
            SoundAudio.PlayWrong();
            return;
        }
        Model = Core.Turn(new GameTurnInput() { Move = move });
        UpdateScreenAfterAction();
        CheckBonusWinOrLoss();
    }

    internal void QuitAction()
    {
        view.QuitDialog.SetActive(true);
    }

    internal void QuitConfirmAction()
    {
        Core.State = GameState.Loss;
        SceneManager.LoadScene(Globals.SCENE_MAP, LoadSceneMode.Single);
    }

    internal void QuitCancelAction()
    {
        view.QuitDialog.SetActive(false);
    }

    internal void WinAction()
    {
        PlayerPrefs.Save();
        Globals.LEVEL = Main.NextLevel(Globals.LEVEL);
        SceneManager.LoadScene(Globals.SCENE_GAME, LoadSceneMode.Single);
    }

    internal void OpenDialogAction(CardView card)
    {
        CardDialog.GetComponent<CardDialog>()
            .Card = card;
        CardDialog.SetActive(true);
    }

    // ********************************************************************

    private void LoadData()
    {
        state.levelData = JsonUtility.FromJson<LevelData>(
            Resources.Load<TextAsset>(
                Globals.LEVEL.path + "/" + "data")
                    .text);
    }

    private void UpdateScreenAfterAction()
    {
        IEnumerator coroutine = UpdateScreenRoutine();
        StartCoroutine(coroutine);
    }

    private IEnumerator UpdateScreenRoutine()
    {
        yield return new WaitForSeconds(0.0f);
        UpdateSprites();
        UpdateButtons();
        UpdateMoves();
        UpdateScore();
        if (HasMerged())
        {
            CreateMoveAnimations();
            CreateBonusAnimations();
            SoundAudio.PlayMerge();
        }
        else if (HasMoved())
        {
            CreateMoveAnimations();
            SoundAudio.PlayMove();
        }
    }

    private void InitComponents()
    {
        view.QuitDialog.SetActive(false);
        view.QuitDialog.AddComponent<BringToFront>();
        view.QuitDialog.AddComponent<QuitDialog>()
            .QuitAction = QuitConfirmAction;
        view.LooseDialog.SetActive(false);
        LooseDialog loss = view.LooseDialog.AddComponent<LooseDialog>();
        loss.ConfirmAction = QuitConfirmAction;
        loss.CancelAction = UndoAction;
        view.WinDialog1.SetActive(false);
        view.WinDialog1.AddComponent<WinDialog1>()
            .Action = WinAction;
        TileMovePrefab = gameObject.FindChild("TileMovePrefab", true);
        TileMovePrefab.SetActive(false);
        gameObject.FindChild("CardsButton")
            .GetComponent<Button>()
                .onClick.AddListener(
                    () => OpenDialogAction(new CardView()
                    {
                        id = Globals.LEVEL.id,
                        path = Globals.LEVEL.path,
                        objective = 0,
                    }));
    }

    private void InitAudio() 
    {
        AudioSource SoundAudioSource = gameObject.AddComponent<AudioSource>();
        SoundAudioSource.mute = !PlayerPrefsHelper.GetBool(PlayerPrefsHelper.SOUND);

        SoundAudio = gameObject.AddComponent<SoundAudio>();
        SoundAudio.AudioSource = SoundAudioSource;
        SoundAudio.PlayIntroAtStartup = true;

        SettingsButton SettingsButton = gameObject.FindChild("SettingsButton")
            .AddComponent<SettingsButton>();
        SettingsButton.TranslateTarget = GameObject.Find("UICanvas");
        SettingsButton.SoundAudioSource = SoundAudioSource;
    }

    private void InitBindView()
    {
        view = new GameSceneView()
        {
            UICanvas = GameObject.Find("UICanvas"),
            Camera = GameObject.Find("Main Camera"),
            UndoButton = gameObject.FindChild("UndoButton"),
            BackgroundSprite = gameObject.FindChild("BackgroundSprite"),
            WallpaperSprite = gameObject.FindChild("WallpaperSprite"),
            BonusAnimation = gameObject.FindChild("MergeAnimation", true),
            CompletionValue = gameObject.FindChild("CompletionText"),
            CompletionPreviewImage = gameObject.FindChild("CompletionPreviewImage"),
            QuitDialog = gameObject.FindChild("QuitDialog", true),
            LooseDialog = gameObject.FindChild("LooseDialog", true),
            WinDialog1 = gameObject.FindChild("WinDialog1", true)
        };
        view.GameMoves = new Dictionary<string, GameObject>();
        foreach (Movement move in Enum.GetValues(typeof(Movement)))
            foreach (bool b in new bool[] { true, false })
            {
                string name = String.Format(Globals.GAMEOBJECT_MOVE, move, b);
                GameObject gg = GameObject.Find(name);
                gg.SetActive(false);
                view.GameMoves.Add(name, gg);
            }
        view.GameWrong = new Dictionary<Movement, GameObject>();
        foreach (Movement move in Enum.GetValues(typeof(Movement)))
        {
            string name = String.Format(Globals.GAMEOBJECT_MOVE_WRONG, move);
            GameObject gg = GameObject.Find(name);
            gg.SetActive(false);
            MoveInTimeDirection anim = gg.AddComponent<MoveInTimeDirectionThenDestroy>();
            anim.time_for_move = props.WrongAnimationTime;
            switch (move)
            {
                case Movement.Up:
                    anim.move = new Vector2(30, 30);
                    break;
                case Movement.Down:
                    anim.move = new Vector2(-30, -30);
                    break;
                case Movement.Left:
                    anim.move = new Vector2(-30, 30);
                    break;
                case Movement.Right:
                    anim.move = new Vector2(30, -30);
                    break;
            }
            view.GameWrong.Add(move, gg);
        }
        view.TileObjects = new Dictionary<string, GameObject>();
        for (int y = 0; y < Globals.HEIGHT; y++)
            for (int x = 0; x < Globals.WIDTH; x++)
            {
                view.TileObjects.Add(
                    view.TileName(y, x),
                    GameObject.Find(
                        view.TileName(y, x)));
                view.TileObjects.Add(
                    view.TileName(y, x) + " base",
                    GameObject.Find(
                        view.TileName(y, x) + " base"));
            }
        WinAnimation = gameObject.FindChild("WinAnimation", true);
        WinAnimation.SetActive(false);
        CardDialog = Instantiate(
            Resources.Load("Prefabs/CardDialog", typeof(GameObject)),
            GameObject.Find("UICanvas").transform) as GameObject;
        CardDialog.SetActive(false);
        CardDialog.AddComponent<CardDialog>();
    }

    private void InitListeners()
    {
        gameObject.FindChild("QuitButton").OnClick(QuitAction);
        gameObject.FindChild("UndoButton").OnClick(UndoAction);
        gameObject.FindChild("GameHelpButton").OnClick(HelpOpenAction);
        GameObject board = view.UICanvas.FindChild("UIBoard");
        InputDetector input;
        input = board.AddComponent<KeysArrowDetector>();
        input.Left = () => MoveAction(Movement.Left);
        input.Right = () => MoveAction(Movement.Right);
        input.Up = () => MoveAction(Movement.Up);
        input.Down = () => MoveAction(Movement.Down);
        input = board.AddComponent<MouseSwipeDetectorDiagonal>();
        input.Left = () => MoveAction(Movement.Left);
        input.Right = () => MoveAction(Movement.Right);
        input.Up = () => MoveAction(Movement.Up);
        input.Down = () => MoveAction(Movement.Down);
    }

    private void InitStyle()
    {
        Main.Lang.Apply(GameObject.Find("UICanvas"), true);
        Main.Theme.Apply(GameObject.Find("UICanvas"), true);
        Color color = ColorHelper.HEXToRGB(state.levelData.color);
        Sprite sprite = Resources.Load<Sprite>(
            Globals.LEVEL.path + "/" + "wallpaper");
        SpriteRenderer sr;
        sr = view.BackgroundSprite.GetComponent<SpriteRenderer>();
        sr.color = color;
        sr = view.WallpaperSprite.GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
    }

    private void InitAnimations()
    {
        ScreenTransitionAnimator.InitShade(
           Instantiate(new GameObject("shade"), GameObject.Find("UICanvas").transform))
                   .AddComponent<ScreenOpenAnimator>();
        var script = view.BonusAnimation.AddComponent<MoveInTimeDirectionThenDestroy>();
        script.move = new Vector3(0, 50, 0);
        script.time_for_move = props.BonusAnimationTime;
        foreach (Movement move in Enum.GetValues(typeof(Movement)))
            gameObject.FindChild(String.Format(Globals.GAMEOBJECT_MOVE, move, true), true)
                .AddComponent<BlinkAnimator>();
        WinAnimation
            .AddComponent<MoveInTimeDirectionThenDestroy>()
                .time_for_move = props.WinAnimationTime;
        TileMovePrefab
            .AddComponent<MoveInTimeThenDestroy>()
                .time_for_move = props.MoveAnimationTime;
    }

    private void StartMoveWrongAnimation(Movement move)
    {
        GameObject prefab = view.GameWrong[move];
        GameObject animGO = Instantiate(prefab, prefab.transform.parent);
        animGO.SetActive(true);
    }

    private bool HasWon()
    {
        if (Globals.LEVEL.unlimited)
            return false;
        bool hasReachMaxTile = false;
        ForEachIn2DArray(Model.Board, (item, y, x) =>
        {
            if (item.Value >= Globals.LEVEL.objective)
                hasReachMaxTile = true;
        });
        return hasReachMaxTile;
    }

    private bool HasLost()
    {
        return (Model.AvailableMoves.Length == 0);
    }

    private void CheckBonusWinOrLoss()
    {
        if (HasWon())
        {
            SaveProgress();
            SoundAudio.PlayWin();
            CreateWinAnimations();
            DisableGameInputs();
            IEnumerator coroutine = WinCoroutine();
            StartCoroutine(coroutine);
        }
        else
        if (HasProgressed())
        {
            SaveProgress();
            int value = PlayerPrefs.GetInt(state.levelId);
            if (value > Globals.ACHIV_NEWTILE_ANIM_THRESHOLD)
                CreateNewMaxTileAnimations();
        }
        if (HasLost())
        {
            SoundAudio.PlayLoss();
            view.LooseDialog.SetActive(true);
        }
    }
    private void SaveProgress()
    {
        int actual_value = PlayerPrefs.GetInt(state.levelId);
        ForEachIn2DArray(Model.Board, (item, y, x) =>
        {
            if (item.Value > actual_value)
                PlayerPrefs.SetInt(state.levelId, item.Value);
        });
        PlayerPrefs.Save();
    }

    bool HasProgressed()
    {
        int actual_value = PlayerPrefs.GetInt(state.levelId);
        bool cond = false;
        ForEachIn2DArray(Model.Board, (item, y, x) =>
        {
            if (item.Value > actual_value)
                cond = true;
        });
        return cond;
    }

    private void DisableGameInputs()
    {
        foreach (InputDetector sc in gameObject.GetComponentsInChildren<InputDetector>())
            Destroy(sc);
    }

    private IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(props.WinAnimationTime);
        view.WinDialog1.SetActive(true);
    }

    private void StartGame()
    {
        Model = Core.Start(new GameStartInput()
        {
            Height = Globals.HEIGHT,
            Width = Globals.WIDTH
        });
    }

    private bool HasMoved()
    {
        bool res = false;
        ForEachIn2DArray(Model.Board, (item, y, x) =>
        {
            if (item.Moved)
                res = true;
        });
        return res;
    }

    private bool HasMerged()
    {
        bool res = false;
        ForEachIn2DArray(Model.Board, (item, y, x) =>
        {
            if (item.Merged)
                res = true;
        });
        return res;
    }

    private static void ForEachIn2DArray<T>(T[,] matrix, Action<T, int, int> action)
    {
        for (int y = 0; y < Globals.HEIGHT; y++)
            for (int x = 0; x < Globals.WIDTH; x++)
                action(matrix[y, x], y, x);
    }

    private void CreateMoveAnimations()
    {
        Transform parent = view.UICanvas.transform;
        ForEachIn2DArray(Model.Board, (item, y, x) =>
        {
            if (item.Moved)
            {
                foreach (ItemOrigin origin in item.MovedOrigins)
                {
                    var originGO = view.GetTile(origin.Y, origin.X);
                    var destinGO = view.GetTile(y, x);
                    var sprite = state.TileSprites[origin.Value];
                    InitMoveAnimations(sprite, originGO, destinGO)
                        .SetActive(true);
                }
                item.Moved = false;
                item.MovedOrigins = new ItemOrigin[0];
            }
        });
    }

    private GameObject InitMoveAnimations(Sprite sprite, GameObject originGO, GameObject destinGO)
    {
        var animGO = Instantiate(TileMovePrefab, TileMovePrefab.transform.parent);
        var spriteRdr = animGO.GetComponent<SpriteRenderer>();
        spriteRdr.sprite = sprite;
        spriteRdr.sortingOrder = 999;
        var script = animGO.GetComponent<MoveInTimeThenDestroy>();
        script.begin = new Vector3(
            originGO.transform.position.x,
            originGO.transform.position.y,
            originGO.transform.position.z);
        script.end = new Vector3(
            destinGO.transform.position.x,
            destinGO.transform.position.y,
            destinGO.transform.position.z);
        return animGO;
    }


    private void CreateNewMaxTileAnimations()
    {
        Item res = null;
        int max = 0, res_x = 0, res_y = 0;
        ForEachIn2DArray(Model.Board, (item, y, x) =>
        {
            if (item.Value > max)
            {
                max = item.Value;

                res = item;
                res_x = x;
                res_y = y;
            }
        });
        if (res != null)
            CreateCoinAnimations(res, res_y, res_x);
    }

    private void CreateCoinAnimations(Item item, int y, int x)
    {
        GameObject animGO, tileGO;
        Vector3 screenPosition;
        Camera cam = view.Camera.GetComponent<Camera>();
        // TOD perf ?
        Transform parent = view.UICanvas.transform;
        tileGO = view.GetTile(y, x);
        screenPosition = cam.WorldToScreenPoint(tileGO.transform.position);
        animGO = Instantiate(WinAnimation, parent);
        animGO.transform.position = screenPosition;
        animGO.SetActive(true);
    }

    private void CreateWinAnimations()
    {
        ForEachIn2DArray(Model.Board, (item, y, x) =>
            CreateCoinAnimations(item, y, x));
    }

    private void CreateBonusAnimations()
    {
        GameObject animGO, tileGO;
        Vector3 screenPosition;
        Transform parent = view.UICanvas.transform;
        Camera cam = view.Camera.GetComponent<Camera>();
        ForEachIn2DArray(Model.Board, (item, y, x) =>
        {
            if (item.Merged)
            {
                tileGO = view.GetTile(y, x);
                screenPosition = cam.WorldToScreenPoint(tileGO.transform.position);
                animGO = Instantiate(view.BonusAnimation, parent);
                animGO.GetComponent<Text>().text = "+" + item.Value.ToString();
                animGO.transform.position = screenPosition;
                animGO.SetActive(true);
                item.Merged = false;
            }
        });
    }

    int tileMaxReached = 0;

    private void UpdateScore()
    {
        int tileMax = 2;
        ForEachIn2DArray(Model.Board, (item, y, x) =>
        {
            if (tileMax < item.Value)
                tileMax = item.Value;
        });
        if (tileMax <= tileMaxReached)
        {
            return;
        }
        view.CompletionValue
            .GetComponent<Text>()
                .text = tileMax.ToString();
        view.CompletionPreviewImage
            .GetComponent<Image>()
                .sprite = state.PreviewSprites[tileMax];
        SoundAudio.PlayWin();
        tileMaxReached = tileMax;
    }

    private void UpdateMoves()
    {
        foreach (Movement move in Enum.GetValues(typeof(Movement)))
            foreach (bool b in new bool[] { true, false })
            {
                bool state = this.Model.AvailableMoves.Contains(move);
                view.GameMoves[String.Format(Globals.GAMEOBJECT_MOVE, move, b)]
                    .SetActive(!(state && !b));
            }
    }

    private void InitSprites()
    {
        GameObject itemGO;
        SpriteRenderer spriteRend;
        ForEachIn2DArray(Model.Board, (item, y, x) =>
        {
            itemGO = view.GetTileBase(y, x);
            spriteRend = itemGO.GetComponent<SpriteRenderer>();
            spriteRend.sprite = state.TileSprites[0];
            itemGO = view.GetTile(y, x);
            spriteRend = itemGO.GetComponent<SpriteRenderer>();
            spriteRend.sprite = null;
        });
    }

    private void UpdateSprites()
    {
        GameObject itemGO;
        SpriteRenderer spriteRend;
        Sprite sprite;
        ForEachIn2DArray(Model.Board, (item, y, x) =>
        {
            sprite = (item.Value > 0) ? state.TileSprites[item.Value] : null;
            itemGO = view.GetTile(y, x);
            spriteRend = itemGO.GetComponent<SpriteRenderer>();
            spriteRend.sprite = sprite;
        });
    }

    private void UpdateButtons()
    {
        view.UndoButton.GetComponent<Button>()
            .interactable = Model.CanUndo;
    }

    private void LoadSprites()
    {
        string path;
        state.TileSprites.Clear();
        foreach (string i in Globals.VALUES_TO_SPRITES.Values)
        {
            path = Globals.LEVEL.path + "/" + i;
            state.TileSprites.Add(Int16.Parse(i), Resources.Load<Sprite>(path));
        }
        state.PreviewSprites.Clear();
        foreach (string i in Globals.VALUES_TO_SPRITES.Values)
        {
            path = Globals.LEVEL.path + "/" + i + "-preview";
            state.PreviewSprites.Add(Int16.Parse(i), Resources.Load<Sprite>(path));
        }
    }
}