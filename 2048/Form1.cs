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
using System.IO;

namespace _2048
{
    public partial class Form1 : Form
    {
        private string GameStateFileName = "last_state.txt";
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
            if (TryRepearPreviousState())
            {
                NewGame();
            }
        }

        private bool TryRepearPreviousState()
        {
            try
            {
                using (FileStream fs = File.OpenRead(GameStateFileName))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while(!sr.EndOfStream)
                        {
                            string[] tile = sr.ReadLine().Split(';');
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
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
            SoccerLabel.Text = GameLogick.Soccer.ToString();
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            File.Delete(GameStateFileName);
            using (FileStream fs = File.OpenWrite(GameStateFileName))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    foreach (var item in GameLogick.TilesState)
                    {
                        sw.WriteLine("{0};{1};{2}", item.Key.X, item.Key.Y, item.Value);
                    }
                }
            }
            base.OnFormClosing(e);
        }
    }
}
