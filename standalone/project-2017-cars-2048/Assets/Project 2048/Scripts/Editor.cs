#if (UNITY_EDITOR)

using UnityEditor;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Globalization;
using System.Text.RegularExpressions;

using Project2048;

// http://docs.unity3d.com/Manual/RunningEditorCodeOnLaunch.html
[InitializeOnLoad]
public class Editor
{
    // https://unity3d.com/fr/learn/tutorials/topics/interface-essentials/unity-editor-extensions-menu-items

    [MenuItem("Tools/Project 2048/Demo Board")]
    private static void ApplyDemoBoard()
    {
        Item[,] Board = new Item[,] {
            { new Item(512),new Item(2), new Item(256),new Item(2) },
            { new Item(32),new Item(4), new Item(4),new Item(2) },
            { new Item(16),new Item(2), new Item(2),new Item(8) },
            { new Item(16),new Item(0), new Item(2),new Item(0) }
         };
        Main.Instance.Core.Reload(new Game()
        {
            Width = 4,
            Height = 4,
            Board = Board
        });
    }

    [MenuItem("Tools/Project 2048/Demo Board 2048")]
    private static void ApplyDemoBoard2048()
    {
        Item[,] Board = new Item[,] {
            { new Item(128),new Item(256), new Item(1024),new Item(2048) },
            { new Item(0),new Item(32), new Item(64),new Item(512) },
            { new Item(2),new Item(4), new Item(2),new Item(16) },
            { new Item(0),new Item(0), new Item(2),new Item(8) }
         };
        Main.Instance.Core.Reload(new Game()
        {
            Width = 4,
            Height = 4,
            Board = Board
        });
    }

    [MenuItem("Tools/Project 2048/Demo Board 512")]
    private static void ApplyDemoBoard1024()
    {
        Item[,] Board = new Item[,] {
            { new Item(8),new Item(64), new Item(512),new Item(512) },
            { new Item(0),new Item(32), new Item(64),new Item(16) },
            { new Item(2),new Item(0), new Item(0),new Item(0) },
            { new Item(0),new Item(0), new Item(2),new Item(0) }
         };
        Main.Instance.Core.Reload(new Game()
        {
            Width = 4,
            Height = 4,
            Board = Board
        });
    }

}

#endif