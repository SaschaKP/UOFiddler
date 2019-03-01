using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Ultima;

namespace Region_Editor
{
    public enum Scaling
    {
        Scale2,
        Scale3,
        Scale4,
        Scale5,
        Scale10,
        Scale20,
        Scale30
    }

    public class MapDisplay : PictureBox
    {
        private Point _Upper_Coordinate = new Point(0,0);
        private Point _Hover_Coordinate = new Point(0,0);
        private Scaling _Scale = Scaling.Scale30;
        private Rectangle _HighlightedArea = new Rectangle(0, 0, 0, 0);

        [Bindable(true), Category("Behavior"), DefaultValue(typeof(Point), "0,0"), Description("The coordinate of the UO map in the upper left corner.")]
        public Point Upper_Coordinate
        {
            get { return _Upper_Coordinate; }
            set
            {
                _Upper_Coordinate = value;
                Invalidate();
            }
        }

        [Category("Action"), Description("Occurs when the user moves the mouse over a new coordinate")]
        public event EventHandler HoverCoordinatedChanged;

        [Bindable(true), Category("Behavior"), DefaultValue(typeof(Point), "0,0"), Description("The coordinate of the UO map that the user is hovering over.")]
        public Point Hover_Coordinate
        {
            get { return _Hover_Coordinate; }
        }

        [Bindable(true), Category("Behavior"), DefaultValue(typeof(Scaling), "Full"), Description("The scale at which the map should be displayed.")]
        public Scaling MapScale
        {
            get { return _Scale; }
            set
            {
                _Scale = value;

                switch (_Scale)
                {
                    case Scaling.Scale30:
                        ScaledSize = 30;
                        break;
                    case Scaling.Scale20:
                        ScaledSize = 20;
                        break;
                    case Scaling.Scale10:
                        ScaledSize = 10;
                        break;
                    case Scaling.Scale5:
                        ScaledSize = 5;
                        break;
                    case Scaling.Scale4:
                        ScaledSize = 4;
                        break;
                    case Scaling.Scale3:
                        ScaledSize = 3;
                        break;
                    case Scaling.Scale2:
                        ScaledSize = 2;
                        break;
                }

                _TileWidth = Width / ScaledSize;

                Invalidate();
            }
        }

        [Bindable(true), Category("Behavior"), DefaultValue(typeof(Rectangle), "0,0,0,0"), Description("The area of the UO map that the user has highlighted.")]
        public Rectangle HighlightedArea
        {
            get { return _HighlightedArea; }
            set
            {
                _HighlightedArea = value;
                LeftMousePressed = false;
                HighlightBegin = new Point(_HighlightedArea.X, _HighlightedArea.Y);
                HighlightEnd = new Point(_HighlightedArea.X + _HighlightedArea.Width, _HighlightedArea.Y + _HighlightedArea.Height);
            }
        }

        private Hashtable MapCoordinates = new Hashtable();
        private bool LeftMousePressed = false;
        private Point HighlightBegin = new Point(-1, -1);
        private Point HighlightEnd = new Point(-1, -1);
        private Point LastClicked = new Point(-1, -1);
        private int ScaledSize = 30;

        private int _TileWidth = 18;
        public int TileWidth { get { return _TileWidth; } }

        public MapDisplay()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserMouse, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, false);

            TabStop = false;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            TileMatrix matrix = Parameters.CurrentMap.Tiles;

            bool tilesHighlite = false;
            pe.Graphics.Clear(Color.Black);
            MapCoordinates = new Hashtable();

            for (int x = 0; x < _TileWidth; x++)
            {
                for (int y = 0; y < _TileWidth; y++)
                {
                    int upperX = x * ScaledSize;
                    int upperY = y * ScaledSize;

                    Bitmap bmp = Cache.GetTile(matrix.GetLandTile(_Upper_Coordinate.X + x, _Upper_Coordinate.Y + y).ID, (int)MapScale);

                    if (bmp == null)
                    {
                        bmp = new Bitmap(ScaledSize, ScaledSize);
                        Graphics g = Graphics.FromImage(bmp);
                        g.Clear(Color.Black);
                        g.Dispose();
                    }

                    Rectangle area = new Rectangle(upperX, upperY, ScaledSize, ScaledSize);

                    if (_HighlightedArea.Contains(_Upper_Coordinate.X + x, _Upper_Coordinate.Y + y))
                        tilesHighlite = true;
                    else
                        tilesHighlite = false;

                    pe.Graphics.DrawImage(bmp, upperX, upperY);

                    if (MapScale >= Scaling.Scale10)
                        pe.Graphics.DrawRectangle(new Pen(Color.FromArgb(150, Color.Black)), area);
                    else
                        pe.Graphics.DrawRectangle(new Pen(Color.FromArgb(100, Color.Black)), area);

                    if (tilesHighlite)
                        pe.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Yellow)), area);

                    MapCoordinates[area] = new Point(_Upper_Coordinate.X + x, _Upper_Coordinate.Y + y);
                }
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            if (Parent is RegionEditor)
                ((RegionEditor)Parent).GotoLocation(LastClicked.X, LastClicked.Y);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Point coord = new Point(-1,-1);

            foreach (DictionaryEntry entry in MapCoordinates)
            {
                if (entry.Key is Rectangle && ((Rectangle)entry.Key).Contains(e.Location))
                {
                    if (entry.Value is Point)
                    {
                        coord = new Point(((Point)entry.Value).X, ((Point)entry.Value).Y);
                    }
                }
            }

            LastClicked = coord;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                LeftMousePressed = true;

                if (HighlightBegin.X != -1 && ModifierKeys == Keys.Shift && coord.X != -1)
                {
                    HighlightEnd = coord;
                }
                else
                {
                    if (HighlightBegin.X != -1)
                    {
                        HighlightBegin = new Point(-1, -1);
                        HighlightEnd = new Point(-1, -1);
                        _HighlightedArea = new Rectangle(0, 0, 0, 0);
                        Invalidate();
                    }

                    if (coord.X != -1)
                    {
                        HighlightBegin = coord;
                    }
                }
            }

            if (HighlightBegin.X != -1 && HighlightEnd.X != -1)
            {
                Point start = new Point(HighlightBegin.X, HighlightBegin.Y);
                Point end = new Point(HighlightEnd.X, HighlightEnd.Y);
                FixPoints(ref start, ref end);
                _HighlightedArea = new Rectangle(start.X, start.Y, end.X - start.X + 1, end.Y - start.Y + 1);

                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            LeftMousePressed = false;

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            foreach (DictionaryEntry entry in MapCoordinates)
            {
                if (entry.Key is Rectangle && ((Rectangle)entry.Key).Contains(e.Location))
                {
                    if (entry.Value is Point)
                    {
                        Point coord = new Point(((Point)entry.Value).X, ((Point)entry.Value).Y);

                        if (coord != _Hover_Coordinate)
                        {
                            _Hover_Coordinate = coord;

                            if (HoverCoordinatedChanged != null)
                                HoverCoordinatedChanged(this, new EventArgs());
                        }

                        if (LeftMousePressed)
                        {
                            if (HighlightBegin.X == -1)
                                HighlightBegin = coord;

                            HighlightEnd = coord;

                            Point start = new Point(HighlightBegin.X, HighlightBegin.Y);
                            Point end = new Point(HighlightEnd.X, HighlightEnd.Y);
                            FixPoints(ref start, ref end);
                            _HighlightedArea = new Rectangle(start.X, start.Y, end.X - start.X + 1, end.Y - start.Y + 1);

                            Invalidate();
                        }
                    }
                }
            }

            base.OnMouseMove(e);
        }

        private void FixPoints(ref Point top, ref Point bottom)
        {
            if (bottom.X < top.X)
            {
                int swap = top.X;
                top.X = bottom.X;
                bottom.X = swap;
            }

            if (bottom.Y < top.Y)
            {
                int swap = top.Y;
                top.Y = bottom.Y;
                bottom.Y = swap;
            }
        }
    }
}
