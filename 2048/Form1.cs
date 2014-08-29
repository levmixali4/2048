using _2048.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2048.Core;

namespace _2048
{
    public partial class Form1 : Form
    {
        const int Side = 4;
        Dictionary<Coordinate, Tile> _tiles;
        GameLogick GameLogick; 
        Random _random;
        Random Random
        {
            get
            {
                if (_random == null)
                {
                    _random = new Random();
                }
                return _random;
            }
        }

        public Form1()
        {
            InitializeComponent();
            KeyPreview = true;
            GameLogick = new GameLogick(Side);
            GameLogick.StateChanged += GameLogick_StateChanged;
            GameLogick.GameOver += GameLogick_GameOver;
            GameLogick.NewGame();
        }

        void GameLogick_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (Tiles.Count == 0)
            {
                foreach (var item in GameLogick.TilesState)
                {
                    Tile tile = new Tile(item.Key);
                    tile.Value = item.Value;
                    MainPanel.Controls.Add(tile);
                    tile.Location = new Point((item.Key.X * tile.Width - tile.Width) + tile.Margin.Left * item.Key.X, (item.Key.Y * tile.Height - tile.Height) + tile.Margin.Top * item.Key.Y);
                    Tiles.Add(item.Key, tile);
                }
            }
            else
            {
                foreach (var item in GameLogick.TilesState)
                {
                    Tiles[item.Key].Value = item.Value;
                }
            }

            for (int i = 0; i < e.NewCoordinates.Length; i++)
            {
                Tiles[e.NewCoordinates[i]].Blink();
            }
        }

        void GameLogick_GameOver(object sender, EventArgs e)
        {
            StatusLabel.Text = "Game Over";
        }

        private Dictionary<Coordinate, Tile> Tiles
        {
            get
            {
                if (_tiles == null)
                {
                    _tiles = new Dictionary<Coordinate, Tile>(new CoordinateEqualityComparer());
                }
                return _tiles;
            }
        }

        private void NewGame()
        {
            StatusLabel.Text = string.Empty;
            GameLogick.NewGame();            
        }

        //private void AddRandomValue()
        //{
        //    int x, y;
        //    Tile tile = null;
        //    Coordinate c;
        //    while (tile == null || tile.Value != 0)
        //    {
        //        x = Random.Next(1, Side+1);
        //        y = Random.Next(1, Side+1);
        //        c = new Coordinate(x, y);
        //        tile = Tiles[c];
        //    }
        //    if (Random.Next(-5, 5) >= 0)
        //        tile.Value = 2;
        //    else
        //        tile.Value = 4;

        //    if (IsGameOver())
        //    {
        //        StatusLabel.Text = "Game Over";
        //    }
        //}

        //private bool IsGameOver()
        //{
        //    if (Tiles.Any(t => t.Value.Value == 0))
        //        return false;

        //    return !AnyMoves(ArrowDirection.Left) && !AnyMoves(ArrowDirection.Up);
        //}

        //private bool AnyMoves(ArrowDirection direction)
        //{
        //    int x = 1, y = 1;
        //    Coordinate c = new Coordinate();
        //    Coordinate cNext = new Coordinate();
        //    for (int i = 1; i <= Side; i++)
        //    {
        //        if (direction == ArrowDirection.Up)
        //        {
        //            x = i;
        //        }
        //        else
        //        {
        //            y = i;
        //        }
        //        c = new Coordinate(x, y);
        //        TryGetNextCoordinate(direction, c, out cNext);
        //        while (true)
        //        {
        //            if (Tiles[c].Value == Tiles[cNext].Value)
        //            {
        //                return true;
        //            }
        //            if (!TryGetNextCoordinate(direction, c, out c) || !TryGetNextCoordinate(direction, cNext, out cNext))
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    return false;
        //}

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    GameLogick.ExecuteMove(Direction.Up);
                    return;
                case Keys.Right:
                    GameLogick.ExecuteMove(Direction.Right);                        
                    return;
                case Keys.Down:
                    GameLogick.ExecuteMove(Direction.Down);
                    return;
                case Keys.Left:
                    GameLogick.ExecuteMove(Direction.Left);
                    return;
                default:
                    return;
            }
        }
        
        //private bool ExecuteMove(ArrowDirection direction, int x, int y)
        //{
        //    Coordinate c, cNext, cEmpty;
        //    Tile tile, tileNext;
        //    bool wasMoved = false;

        //    for (int i = 1; i <= Side; i++)
        //    {
        //        if (direction == ArrowDirection.Down || direction == ArrowDirection.Up)
        //        {
        //            x = i;
        //        }
        //        else
        //        {
        //            y = i;
        //        }
        //        c = new Coordinate(x, y);
        //        TryGetNextCoordinate(direction, c, out cNext);
        //        cEmpty = null;
        //        while (true)
        //        {
        //            tile = Tiles[c];
        //            tileNext = Tiles[cNext];
        //            if (tile.Value == 0 && tileNext.Value == 0)
        //            {
        //                if (cEmpty == null)
        //                    cEmpty = cNext;
        //                if (!TryGetNextCoordinate(direction, cNext, out cNext))
        //                {
        //                    break;
        //                }
        //                continue;
        //            }
        //            else if (tile.Value != 0 && tileNext.Value == 0)
        //            {
        //                if (cEmpty == null)
        //                    cEmpty = cNext;
        //                if (!TryGetNextCoordinate(direction, cNext, out cNext))
        //                {
        //                    break;
        //                }
        //                continue;
        //            }
        //            else if (tile.Value == 0 && tileNext.Value != 0)
        //            {
        //                tile.Value = tileNext.Value;
        //                tileNext.Value = 0;
        //                wasMoved = true;
        //                if (cEmpty == null)
        //                    cEmpty = cNext;
        //                if (!TryGetNextCoordinate(direction, cNext, out cNext))
        //                {
        //                    break;
        //                }
        //                continue;
        //            }
        //            else if (tile.Value != 0 && tileNext.Value != 0)
        //            {
        //                if (tile.Value == tileNext.Value)
        //                {
        //                    tile.Value = tile.Value + tileNext.Value;
        //                    tileNext.Value = 0;
        //                    wasMoved = true;
        //                    if (!TryGetNextCoordinate(direction, c, out c) || !TryGetNextCoordinate(direction, cNext, out cNext))
        //                    {
        //                        break;
        //                    }
        //                }
        //                else if (cEmpty != null)
        //                {
        //                    Tiles[cEmpty].Value = tileNext.Value;
        //                    tileNext.Value = 0;
        //                    wasMoved = true;
        //                    if (!TryGetNextCoordinate(direction, cEmpty, out c) || !TryGetNextCoordinate(direction, cNext, out cNext))
        //                    {
        //                        break;
        //                    }
        //                    cEmpty = null;
        //                }
        //                else
        //                {
        //                    if (!TryGetNextCoordinate(direction, c, out c) || !TryGetNextCoordinate(direction, cNext, out cNext))
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return wasMoved;
        //}

        //private bool TryGetNextCoordinate(ArrowDirection direction, Coordinate c, out Coordinate cNext)
        //{
        //    switch (direction)
        //    {
        //        case ArrowDirection.Down:
        //            if (c.Y == 1)
        //            {
        //                cNext = c;
        //                return false;
        //            }
        //            cNext = new Coordinate(c.X, c.Y - 1);
        //            return true;                    
        //        case ArrowDirection.Left:
        //            if (c.X == 4)
        //            {
        //                cNext = c;
        //                return false;
        //            }
        //            cNext = new Coordinate(c.X + 1, c.Y);
        //            return true;                    
        //        case ArrowDirection.Right:
        //            if (c.X == 1)
        //            {
        //                cNext = c;
        //                return false;
        //            }
        //            cNext = new Coordinate(c.X - 1, c.Y);
        //            return true;
        //        case ArrowDirection.Up:
        //            if (c.Y == Side)
        //            {
        //                cNext = c;
        //                return false;
        //            }
        //            cNext = new Coordinate(c.X, c.Y + 1);
        //            return true;
        //    }
        //    cNext = c;
        //    return false;
        //}
    }
}
