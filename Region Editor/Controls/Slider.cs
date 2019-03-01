using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Region_Editor
{
    public partial class Slider : UserControl
    {
        private int _minValue = 0;
        private int _maxValue = 100;
        private int _smallChange = 1;
        private int _value = 0;
        private bool _dragModeEnabled = false;
        private Point _startDragPoint;
        private Point _clickPoint;

        [Bindable(true), Category("Behavior"), DefaultValue(0), Description("The minimum value of the slider")]
        public int Minimum
        {
            get { return _minValue; }
            set
            {
                _minValue = value;

                if (_value < _minValue)
                    Value = _minValue;
                else
                {
                    MoveSlider();
                    Invalidate();
                }
            }
        }

        [Bindable(true), Category("Behavior"), DefaultValue(0), Description("The value of the slider")]
        public int Value
        {
            get { return _value; }
            set
            {
                if (value < _minValue || value > _maxValue)
                    return;

                if (value == _value)
                    return;

                int tmp = _value;
                _value = value;
                MoveSlider();
            }
        }

        [Bindable(true), Category("Behavior"), DefaultValue(100), Description("The maximum value of the slider")]
        public int Maximum
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;

                if (_value > _maxValue)
                    Value = _maxValue;
                else
                {
                    MoveSlider(); 
                    Invalidate();
                }
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(1), Description("The number of positions the slider moves by clicking")]
        public int SmallChange
        {
            get { return _smallChange; }
            set
            {
                _smallChange = value;
            }
        }

        private Orientation _Orientation = Orientation.Horizontal;

        [Bindable(true), Category("Behavior"), DefaultValue(typeof(Orientation), "Horizontal"), Description("The orientation of the slider control.")]
        public Orientation Orientation
        {
            get { return _Orientation; }
            set
            {
                if (_Orientation != value)
                {
                    _Orientation = value;

                    Size size;

                    switch (_Orientation)
                    {
                        case System.Windows.Forms.Orientation.Horizontal:
                            size = new Size(Height, Width);
                            MinimumSize = new Size(100, 22);
                            MaximumSize = new Size(1000, 22);
                            Size = size;

                            break;
                        case System.Windows.Forms.Orientation.Vertical:
                            size = new Size(Height, Width);
                            MinimumSize = new Size(22, 100);
                            MaximumSize = new Size(22, 1000);
                            Size = size;

                            break;
                    }

                    OnResize(new EventArgs());
                }
            }
        }

        [Category("Action"), Description("Occurs when the slider is moved")]
        public event EventHandler ValueChanged;

        [Category("Action"), Description("Occurs when the user is done moving the slider")]
        public event EventHandler SliderMoveComplete;

        public Slider()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            SetStyle(ControlStyles.UserMouse, true);
            SetStyle(ControlStyles.UserPaint, true);

            TabStop = false;

            slideControl.Left = 0;
            slideControl.Invalidate();
            MoveSlider();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (_Orientation == System.Windows.Forms.Orientation.Horizontal)
            {
                slideControl.Size = new System.Drawing.Size(10, 20);
                slideControl.Left = 0;
                slideControl.Top = 1;
            }
            else
            {
                slideControl.Size = new System.Drawing.Size(20, 10);
                slideControl.Left = 1;
                slideControl.Top = 0;
            }

            slideControl.Invalidate();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(BackColor);

            if (_Orientation == Orientation.Horizontal)
            {
                e.Graphics.DrawLine(Pens.DarkGray, 0, (Height / 2), Width, (Height / 2));
                e.Graphics.DrawLine(Pens.LightGray, 0, (Height / 2) + 1, Width, (Height / 2) + 1);
            }
            else
            {
                e.Graphics.DrawLine(Pens.DarkGray, (Width / 2), 0, (Width / 2), Height);
                e.Graphics.DrawLine(Pens.LightGray, (Width / 2) + 1, 0, (Width / 2) + 1, Height);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            _clickPoint = e.Location;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            int newValue = _value;

            if (_Orientation == System.Windows.Forms.Orientation.Horizontal)
            {
                if (_clickPoint.X < slideControl.Left)
                    newValue -= _smallChange;
                else
                    newValue += _smallChange;

                if (newValue < _minValue)
                    newValue = _minValue;
                else if (newValue > _maxValue)
                    newValue = _maxValue;
            }
            else
            {
                if (_clickPoint.Y < slideControl.Top)
                    newValue -= _smallChange;
                else
                    newValue += _smallChange;

                if (newValue < _minValue)
                    newValue = _minValue;
                else if (newValue > _maxValue)
                    newValue = _maxValue;
            }

            Value = newValue;

            if (SliderMoveComplete != null)
                SliderMoveComplete(this, new EventArgs());
        }

        private void MoveSlider(int delta)
        {
            if (_Orientation == System.Windows.Forms.Orientation.Horizontal)
            {
                if (delta < 0 && (slideControl.Left + delta) <= 0)
                    slideControl.Left = 0;
                else if (delta > 0 && (slideControl.Left + slideControl.Width + delta) >= Width)
                    slideControl.Left = Width - slideControl.Width;
                else
                    slideControl.Left += delta;
            }
            else
            {
                if (delta < 0 && (slideControl.Top + delta) <= 0)
                    slideControl.Top = 0;
                else if (delta > 0 && (slideControl.Top + slideControl.Height + delta) >= Height)
                    slideControl.Top = Height - slideControl.Height;
                else
                    slideControl.Top += delta;
            }

            CalculateSliderValue();
        }

        private void MoveSlider()
        {
            if (_Orientation == System.Windows.Forms.Orientation.Horizontal)
            {
                float valueLength = (float)(Width - slideControl.Width) / (float)(_maxValue - _minValue);

                slideControl.Left = (int)(valueLength * (float)_value);
            }
            else
            {
                float valueLength = (float)(Height - slideControl.Height) / (float)(_maxValue - _minValue);

                slideControl.Top = (int)(valueLength * (float)_value);
            }
        }

        private void CalculateSliderValue()
        {
            if (_Orientation == System.Windows.Forms.Orientation.Horizontal)
            {
                float valueLength = (float)(_maxValue - _minValue) / (float)(Width - slideControl.Width);

                _value = (int)((float)slideControl.Left * valueLength);
            }
            else
            {
                float valueLength = (float)(_maxValue - _minValue) / (float)(Height - slideControl.Height);

                _value = (int)((float)slideControl.Top * valueLength);
            }

            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());
        }

        private void slideControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            int w = slideControl.Width - 1;
            int h = slideControl.Height - 1;

            e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), 0, 0, w, h);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlLightLight), 0, 0, w, 0);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlLightLight), 0, 0, 0, h);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlLight), 1, 1, w - 1, 1);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlLight), 1, 1, 1, h - 1);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlDarkDark), w, h, w, 1);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlDarkDark), w, h, 1, h);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlDark), w - 1, h - 1, w - 1, 2);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlDark), w - 1, h - 1, 2, h - 1);
        }

        private void slideControl_MouseDown(object sender, MouseEventArgs e)
        {
            _dragModeEnabled = true;
            _startDragPoint = new Point(e.X, e.Y);
        }

        private void slideControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._dragModeEnabled == false)
                return;

            int delta = 0;

            if (_Orientation == System.Windows.Forms.Orientation.Horizontal)
                delta = e.X - this._startDragPoint.X;
            else
                delta = e.Y - this._startDragPoint.Y;

            if (delta == 0)
                return;

            this.MoveSlider(delta);
        }

        private void slideControl_MouseUp(object sender, MouseEventArgs e)
        {
            _dragModeEnabled = false;

            if (SliderMoveComplete != null)
                SliderMoveComplete(this, new EventArgs());
        }

        public void InvokeValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(sender, e);
        }

        public void InvokeSliderMoveComplete(object sender, EventArgs e)
        {
            if (SliderMoveComplete != null)
                SliderMoveComplete(sender, e);
        }
    }
}