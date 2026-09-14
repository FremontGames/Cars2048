using System;
using System.Collections.Generic;
using UnityEngine;
using Project2048;

class GameSceneView
{
    public GameObject Camera;
    public GameObject UICanvas;
    public GameObject BackgroundSprite;
    public GameObject WallpaperSprite;

    public GameObject CompletionValue;
    public GameObject CompletionPreviewImage;

    public GameObject UndoButton;
    public GameObject QuitDialog;

    public GameObject BonusAnimation;

    public Dictionary<string, GameObject> TileObjects;
    public Dictionary<string, GameObject> GameMoves;
    public Dictionary<Movement, GameObject> GameWrong;

    public GameObject LooseDialog;
    public GameObject WinDialog1;

    public string TileName(int y, int x)
    {
        return String.Format("{0}x{1}",
                y, x);
    }

    public GameObject GetTile(int y, int x)
    {
        return TileObjects[
            TileName(y, x)];
    }

    public GameObject GetTileBase(int y, int x)
    {
        return TileObjects[
            TileName(y, x) + " base"];
    }

}
