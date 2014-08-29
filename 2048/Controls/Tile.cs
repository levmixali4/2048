using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _2048.Core;

namespace _2048.Controls
{
    public partial class Tile : UserControl
    {
        Timer _timer;
        Color _originalColor, _defaultColor;
        int _value;
        readonly Coordinate _coordinate;
        public Tile(Coordinate coordinate)
        {
            InitializeComponent();
            _defaultColor = this.BackColor;
            _timer = new Timer();
            _timer.Interval = 500;
            _timer.Tick += _timer_Tick;
            Value = 0;
            _coordinate = coordinate;
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            this.BackColor = _originalColor;
        }

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _timer.Stop();
                UpdateText();
                UpdateFont();
                UpdateColor();
            }
        }

        public void UpdateText()
        {
            string strValue = Value.ToString();
            if (Value == 0)
                strValue = string.Empty;
            ValueLabel.Text = strValue;
        }

        public void UpdateFont()
        {            
            float fontSize = 16f;
            string strValue = Value.ToString();
            if (strValue.Length == 3)
            {
                fontSize = 14f;
            }
            if (strValue.Length == 4)
            {
                fontSize = 11f;
            }

            Font font = ValueLabel.Font;
            Font f = new Font(font.FontFamily, fontSize, font.Style);
            ValueLabel.Font = f;
        }

        public void UpdateColor()
        {            
            this.BackColor = ValueColors.GetValueColor(Value).BackColor;
            this.ForeColor = ValueColors.GetValueColor(Value).TextColor;
        }

        public void Blink()
        {
            return;
            _originalColor = this.BackColor;
            this.BackColor = Color.AliceBlue;
            _timer.Start();
        }

        public Coordinate Coordinate
        {
            get
            {
                return _coordinate;
            }
        }
    }
}
