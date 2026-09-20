/* Copyright (C) 2017 Damien Fremont - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary
 * Written by Damien Fremont
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project2048
{
    // API

    public interface IGameManager
    {
        Game Start(GameStartInput input);
        Game Turn(GameTurnInput input);
        Game Reload(Game input);
    }

    // DATA

    public class Game
    {
        public int Width;
        public int Height;
        public Item[,] Board;

        public int Score;
        public GameState State;
        public Movement[] AvailableMoves;
        public Movement LastMove;
        public bool CanUndo;
    }
    public class GameStartInput
    {
        public int Width;
        public int Height;
    }
    public class GameTurnInput
    {
        public Movement Move;
    }

    public enum GameState { Playing, Won, Loss }
    public enum Movement { Left, Right, Up, Down }
    public class Item
    {
        public int Value;
        public Item(int value) { Value = value; }
        public bool Merged;
        public bool Moved;
        public ItemOrigin[] MovedOrigins;

        public Item(int value,
            bool merged,
            bool moved, ItemOrigin[] origins)
        {
            Value = value;
            Merged = merged;
            Moved = moved;
            MovedOrigins = origins;
        }
    }

    public class ItemOrigin
    {
        public int Value;
        public int X;
        public int Y;
    }
}
