using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commons.UI;

using UnityEngine;

class Globals
{
    // VARS *********************************

    public const int QUALITY_FRAMERATE = 30;
    public const int QUALITY_VSYNC = 1; // 0 for no sync, 1 for panel refresh rate, 2 for 1/2 panel rate
    public static string THEME = "2048Theme";
    public static ChapterData LEVEL = new ChapterData()
    {
        id = 101,
        path = "Levels/f40",
        objective = 2048,
        unlimited = false
    };

    // CONST *********************************

    public const int HEIGHT = 4;
    public const int WIDTH = 4;
    public const int CARDS_BY_LEVEL = 12;
    public const int ACHIV_COINS_VALUE = 1;
    public const int ACHIV_TUTORIAL_THRESHOLD = 15;
    public const int ACHIV_NEWTILE_ANIM_THRESHOLD = 2;
    public const float TIME_BETWEEN_ADS = 15.0f;

    public const string SCENE_MAIN = "Main";
    public const string SCENE_MAP = "Menu";
    public const string SCENE_GAME = "Game";
    public const string SCENE_CARDS = "Cards";
    
    public const string GAMEOBJECT_MOVE = "Move {0} {1}";
    public const string GAMEOBJECT_MOVE_WRONG = "Move {0} Wrong";

    public static Dictionary<int, string> VALUES_TO_SPRITES = new Dictionary<int, string>() { 
            {0, "0000" },
            {2,"0002"},
            {4,"0004"},
            {8,"0008"},
            {16,"0016"},
            {32,"0032"},
            {64,"0064"},
            {128,"0128"},
            {256,"0256"},
            {512,"0512"},
            {1024,"1024"},
            {2048,"2048"} };
}
